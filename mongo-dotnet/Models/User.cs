namespace mongo_dotnet.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

class User : Entity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    
    [BsonElement("location")]
    public GeoJsonPoint<GeoJson2DCoordinates>? Location { get; set; }
}
