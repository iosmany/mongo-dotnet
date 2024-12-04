using MongoDB.Bson.Serialization.Attributes;

namespace mongo_dotnet.Models;

class CinemaMovies : Entity
{
    public required string CinemaId { get; set; }
    public required string MovieId { get; set; }
}
