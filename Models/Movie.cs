
using MongoDotnet.Infrastructure;

namespace MongoDotnet.Models
{
    [MongoDescriptor("movies")]
    public class Movie
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Director { get; set; }
        public int Year { get; set; }
    }
}