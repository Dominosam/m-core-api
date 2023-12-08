using System;
using Microsoft.Extensions.Configuration;
using Mover.Data.Contexts;
using Mover.Data.Repositories.Watch;

namespace Mover.Data.Tests.Repositories
{
    public class RedisRepositoryTests
    {
        private readonly IConfiguration _configuration;
        private readonly RedisContext _redisContext;

        private const string CONNECTION_STRING_KEY = "Redis";

        private readonly string _defaultRepositoryKey = "watch";

        public RedisRepositoryTests()
        {
            // Just for testing purposes, we're using the same appsettings.json file that the Mover.Data project uses
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Mover.Data.Tests", "Configurations", "appsettings.redis.json");

            _configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();

            var connectionString = _configuration.GetConnectionString(CONNECTION_STRING_KEY);

            if (connectionString == null)
            {
                throw new InvalidOperationException("Redis connection string is null.");
            }

            //_redisContext = new RedisContext(connectionString);
        }

        [Fact]
        public void SaveAndRetrieveResponse_ShouldWork()
        {
            // Arrange
            var redisRepository = new WatchHandsRepository(_redisContext);

            var time = DateTime.UtcNow;
            var leastAngle = 42.0;
            var greatestAngle = 60.0;
            var callTimeStamp = DateTime.UtcNow;

            try
            {
                // Act
                //redisRepository.SaveResponse(time, leastAngle, greatestAngle, callTimeStamp);

                // Assert
                var retrievedLeastAngle = redisRepository.GetNewestLeastAngle();

                Assert.NotNull(retrievedLeastAngle);
                Assert.Equal(leastAngle, retrievedLeastAngle);
            }
            finally
            {
                DeleteResponseFromRedis();
            }
        }

        private void DeleteResponseFromRedis()
        {
            var database = _redisContext.GetDatabase();
            database.KeyDelete(_defaultRepositoryKey);
        }
    }

}
