
using mongo_dotnet.Models;
using MongoDotnet.Infrastructure;

namespace MongoDotnet.Models;

[MongoDescriptor("movies")]
class Movie: Entity
{
    public required string Title { get; set; }
    public required string Director { get; set; }
    public int Year { get; set; }
}