using mongo_dotnet.Infrastructure;
using mongo_dotnet.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDotnet.Base;
using System.Linq.Expressions;

namespace mongo_dotnet.Application;

internal class UserService
{
    private readonly IStorageProvider _storageProvider;

    public UserService(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public void Add(User entity)
        => _storageProvider.Save(entity);

    public void Update(User entity)
        => _storageProvider.Save(entity);

    public void Delete(ObjectId id)
         => _storageProvider.Delete<User>(id);

    public List<User> Search(string name, string language)
        => _storageProvider
        .Search<User>(name, language)
        .ToList();

    public List<User> FindNearLocation(double longitude, double latitude, double maxDistanceMeters)
    {
        var point = new Point(longitude, latitude);
        var expression = (Expression<Func<User, object?>>)(x => x.Location);
        return _storageProvider
            .FindNear(point, expression, maxDistanceMeters)
            .ToList();
    }
}
