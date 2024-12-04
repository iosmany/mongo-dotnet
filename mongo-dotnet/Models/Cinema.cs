using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace mongo_dotnet.Models
{
    internal class Cinema : Entity
    {
        public required string Name { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public required GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }

        private List<string> _movieIds = new();
        public IReadOnlyCollection<string> MovieIds 
        {
            get => _movieIds;
            set => _movieIds = value.ToList();
        }

    }
}


