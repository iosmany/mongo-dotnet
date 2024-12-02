
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDotnet.Base;
using MongoDB.Bson.Serialization;

namespace MongoDotnet.Infrastructure;

[AttributeUsage(AttributeTargets.Class)]
sealed class MongoDescriptorAttribute : Attribute
{
    public string Collection { get; }
    public MongoDescriptorAttribute(string collection)
    {
        Collection = collection;
    }
}   

public partial class StorageId
{
    public static explicit operator ObjectId(StorageId id)
        => new ObjectId(id.Id);

    public static explicit operator StorageId(ObjectId id)
        => new StorageId(id.ToString());
}

sealed class StorageIdSerializaer : IBsonSerializer<StorageId>
{
    public Type ValueType => typeof(StorageId);

    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;
        var obj = (StorageId)bsonReader.ReadObjectId();
        return obj;
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        var bsonWriter = context.Writer;
        bsonWriter.WriteObjectId((ObjectId)value);
    }

    StorageId IBsonSerializer<StorageId>.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return (StorageId)Deserialize(context, args);
    }

    void IBsonSerializer<StorageId>.Serialize(BsonSerializationContext context, BsonSerializationArgs args, StorageId value)
    {
        Serialize(context, args, value);
    }
}

internal sealed class MongoDbProvider : IStorageProvider
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDbProvider(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _mongoClient = new MongoClient(connectionString);
         var section = configuration.GetSection("MongoDb");
        _mongoDatabase = _mongoClient.GetDatabase(section["DatabaseName"]);

         // Register the custom serializer
        BsonSerializer.RegisterSerializer(new StorageIdSerializaer());
    }

    string CollectionName<E>() where E : class
    {
        var attribute = (MongoDescriptorAttribute?)Attribute.GetCustomAttribute(typeof(E), typeof(MongoDescriptorAttribute));
        return attribute?.Collection ?? typeof(E).Name;
    }

    public void Add<E>(E entity) where E : class
    {
        var collection = _mongoDatabase.GetCollection<E>(CollectionName<E>());
        collection.InsertOne(entity);
    }

    public IEnumerable<E> GetAll<E>() where E : class
    {
        var collection = _mongoDatabase.GetCollection<E>(CollectionName<E>());
        return collection.Find(new BsonDocument()).ToList();
    }
 
    #region Dispose

     public void Dispose()
    {
        _mongoClient?.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    #endregion
}

static class MongoConfig
{
    public const string DatabaseName = "customdb";
    public const string CollectionName = "movies";
}