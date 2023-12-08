using Microsoft.Extensions.Configuration;
using Mover.Data.Interfaces;
using StackExchange.Redis;
using System.Configuration;

namespace Mover.Data.Contexts
{
    public class RedisContext : IRedisContext
    {
        private readonly string _connectionString;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly string _defaultConnectionString = "Redis";

        public RedisContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(_defaultConnectionString);
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Redis connection string is missing or empty.");
            }
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_connectionString);
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}
