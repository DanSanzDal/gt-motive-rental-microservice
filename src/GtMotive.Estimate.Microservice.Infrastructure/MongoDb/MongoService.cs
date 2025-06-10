using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GtMotive.Estimate.Microservice.Infrastructure.Configuration;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB
{
    public interface IMongoService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
        IMongoDatabase Database { get; }
    }

    public sealed class MongoService : IMongoService
    {
        public IMongoDatabase Database { get; }

        public MongoService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            Database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }
    }
}
