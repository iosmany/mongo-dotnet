


namespace MongoDotnet.Infrastructure;

using MongoDB.Bson;

public partial class StorageId
{

    public string Id { get; }
    public StorageId(string id)
    {
        Id = id;
    }


    public static implicit operator StorageId(string id)
        => new StorageId(id);

    public static implicit operator string(StorageId id)
        => id.Id;

    public override bool Equals(object? obj)
    {
        return obj is StorageId id &&
               Id == id.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Id;
    }
}