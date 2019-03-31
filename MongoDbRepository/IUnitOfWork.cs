using MongoDB.Driver;

namespace MongoDbRepository
{
    public interface IUnitOfWork
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
    }
}