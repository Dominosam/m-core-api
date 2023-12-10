using Moq;
using Mover.Core.Watch.Services;
using Mover.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.Tests.Services
{
    public class WatchHandsAngleServiceTests
    {
        [Fact]
        public void CalculateLeastAngleFromTime_ShouldCalculateAndSaveAngle()
        {
            // Arrange
            var repositoryMock = new Mock<IWatchHandsRepository>();
            var service = new WatchHandsAngleService(repositoryMock.Object);
            var testTime = new DateTime(2023, 1, 1, 12, 30, 0); // Replace with your desired time

            // Act
            var result = service.CalculateLeastAngleFromTime(testTime);

            // Assert
            repositoryMock.Verify(repo => repo.SaveWatchResponse(testTime, result), Times.Once);
        }
    }
}
