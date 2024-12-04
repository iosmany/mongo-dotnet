

using mongo_dotnet.Infrastructure;
using mongo_dotnet.Models;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace MongoDotnet.Base
{
    internal interface IStorageProvider : IDisposable, IAsyncDisposable
    {
        void Save<E>(E entity) where E : class, IEntity;
        void Delete<E>(ObjectId id) where E : class, IEntity;
        IEnumerable<E> GetAll<E>() where E : class, IEntity;
        IEnumerable<E> Search<E>(string text, string language = "es") where E: class, IEntity;
        IEnumerable<E> FindNear<E>(Point point, Expression<Func<E, object?>> locationField, double maxDistanceMeters = 100) where E : class, IEntity;
    }
}