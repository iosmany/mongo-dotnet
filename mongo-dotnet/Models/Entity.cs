using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongo_dotnet.Models;

interface IEntity
{
    ObjectId Id { get; }
}

abstract class Entity: IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
}
