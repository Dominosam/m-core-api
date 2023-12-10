using System;
using Microsoft.Extensions.Configuration;
using Moq;
using Mover.Data.Contexts;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Watch;
using StackExchange.Redis;

namespace Mover.Data.Tests.Repositories
{
    public class RedisRepositoryTests
    {
        private readonly Mock<IRedisContext> _redisContextMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly WatchHandsRepository _repository;

        public RedisRepositoryTests()
        {
            _redisContextMock = new Mock<IRedisContext>();
            _databaseMock = new Mock<IDatabase>();
            _redisContextMock.Setup(x => x.GetDatabase()).Returns(_databaseMock.Object);
            _repository = new WatchHandsRepository(_redisContextMock.Object);
        }

        [Fact]
        public void SaveResponse_ShouldSaveWatchBaseToRedis()
        {
            // Arrange
            var time = DateTime.UtcNow;
            var leastAngle = 30.0;

            // Act
            var result = _repository.SaveWatchResponse(time, leastAngle);

            // Assert
            Assert.True(result);
        }
    }

}
