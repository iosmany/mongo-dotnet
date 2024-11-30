

namespace MongoDotnet.Base;

interface IStorageService {
   
    IEnumerable<E> GetAll<E>() where E: class;

}