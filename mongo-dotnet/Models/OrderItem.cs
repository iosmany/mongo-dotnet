using MongoDB.Bson;

namespace mongo_dotnet.Models;

class OrderItem
{
    public ObjectId ProductId { get; set; }
    public int Quantity { get; set; }
}
