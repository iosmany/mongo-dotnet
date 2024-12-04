using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongo_dotnet.Models;

class Order : Entity
{
    [BsonElement("userId")]
    public ObjectId UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
