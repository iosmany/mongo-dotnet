
namespace MongoDotnet.Services;
using MongoDotnet.Base;

sealed class StorageService: IStorageService {

    private readonly IStorageProvider _provider;
    public StorageService(IStorageProvider provider)
    {
        _provider = provider;
    }
    
    public IEnumerable<E> GetAll<E>() where E: class
    {
        return _provider.GetAll<E>();
    }
}