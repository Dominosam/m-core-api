using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Mover.Data.Interfaces;

namespace Mover.Data.Contexts
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly string _connectionString;
        private readonly IMongoDatabase _database;
        private readonly string _defaultConnectionString = "MongoDB";

        public MongoDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(_defaultConnectionString);
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("MongoDB connection string is missing or empty.");
            }
        }

        public IMongoDatabase GetDatabase()
        {
            var url = new MongoUrl(_connectionString);
            var client = new MongoClient(url);

            return client.GetDatabase(url.DatabaseName);
        }

    }
}
