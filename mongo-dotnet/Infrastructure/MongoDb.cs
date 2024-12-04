
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDotnet.Base;
using MongoDB.Bson.Serialization;
using mongo_dotnet.Models;
using MongoDB.Driver.GeoJsonObjectModel;
using mongo_dotnet.Infrastructure;
using System.Formats.Tar;
using System.Linq.Expressions;

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

    E GetById<E>(IMongoCollection<E> collection, ObjectId id) where E: class, IEntity
    {
        return collection
            .Find(Builders<E>.Filter.Eq(u => u.Id, id))
            .FirstOrDefault();
    }

    string CollectionName<E>() where E : class
    {
        var attribute = (MongoDescriptorAttribute?)Attribute.GetCustomAttribute(typeof(E), typeof(MongoDescriptorAttribute));
        return attribute?.Collection ?? typeof(E).Name;
    }

    IMongoCollection<E> GetCollection<E>() where E : class
    {
        var collection= _mongoDatabase.GetCollection<E>(CollectionName<E>());
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        return collection;
    }

    public void Save<E>(E entity) where E : class, IEntity
    {
        var collection = GetCollection<E>();
        var current = GetById(collection, entity.Id);
        if (current is not null)
        {
            var filter = Builders<E>.Filter.Eq(u => u.Id, entity.Id);
            var updateDefinition = Builders<E>.Update.Set(u => u, entity);
            collection.UpdateOne(filter, updateDefinition);
        }
        else
            collection.InsertOne(entity);
    }

    public IEnumerable<E> GetAll<E>() where E : class, IEntity
        => GetCollection<E>()
        .Find(new BsonDocument())
        .ToList();

    public void Delete<E>(ObjectId id) where E : class, IEntity
    {
        var collection = GetCollection<E>();
        var current = GetById(collection, id);
        if (current is not null)
        {
            var filter = Builders<E>.Filter.Eq(u => u.Id, id);
            collection.DeleteOne(filter);
        }
    }

    public IEnumerable<E> Search<E>(string text, string language = "es") 
        where E : class, IEntity
    {
        var filter = Builders<E>
            .Filter.Text(text, new TextSearchOptions { Language = language });

        var collection = GetCollection<E>();

        return collection
            .Find(filter)
            .ToList();
    }

    public IEnumerable<E> FindNear<E>(Point point, Expression<Func<E, object?>> locationField, double maxDistanceMeters = 100) 
        where E: class, IEntity
    {
        var collection = GetCollection<E>();
        var coordinates = new GeoJson2DCoordinates(point.Longitude, point.Latitude);
        var geoPoint = new GeoJsonPoint<GeoJson2DCoordinates>(coordinates);
        var filter = Builders<E>.Filter.NearSphere(locationField, geoPoint, maxDistance: maxDistanceMeters);
        return collection.Find(filter).ToList();
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