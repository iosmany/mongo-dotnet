using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongo_dotnet.Models;

class Product : Entity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();
}
