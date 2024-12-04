namespace mongo_dotnet.Infrastructure
{
    struct Point
    {
        public double Longitude { get; }
        public double Latitude { get; }

        public Point()
        {
            throw new NotImplementedException();
        }

        public Point(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
