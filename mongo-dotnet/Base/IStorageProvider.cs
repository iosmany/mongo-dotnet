

namespace MongoDotnet.Base
{
    public interface IStorageProvider : IDisposable, IAsyncDisposable
    {
        IEnumerable<E> GetAll<E>() where E : class;
    }
}