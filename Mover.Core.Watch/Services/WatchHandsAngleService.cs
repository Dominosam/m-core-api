using Mover.Core.Watch.Interfaces.Services;
using Mover.Core.Watch.Models.DTOs;
using Mover.Core.Watch.Models.Request;
using Mover.Core.Watch.Models.Response;
using Mover.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.Services
{
    public class WatchHandsAngleService : IWatchHandsAngleService
    {
        private readonly IWatchHandsRepository _repository;
        public WatchHandsAngleService(IWatchHandsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public double CalculateLeastAngleFromTime(DateTime time)
        {
            double leastAngle = CalculateLeastAngleDifference(time);
            _repository.SaveResponse(time, leastAngle);

            return leastAngle;
        }

        public double CalculateLeastAngleFromCurrentTime()
        {
            var currentTime = DateTime.Now;
            double leastAngle = CalculateLeastAngleDifference(currentTime);
            _repository.SaveResponse(DateTime.Now, leastAngle);

            return leastAngle;
        }

        public WatchResponse GetWatchFromTime(DateTime time)
        {
            double leastAngle = CalculateLeastAngleDifference(time);
            _repository.SaveResponse(time, leastAngle);

            return GetResponse(time, leastAngle);
        }

        public WatchResponse GetWatchFromCurrentTime()
        {
            var currentTime = DateTime.Now;
            double leastAngle = CalculateLeastAngleDifference(currentTime);
            _repository.SaveResponse(DateTime.Now, leastAngle);

            return GetResponse(currentTime, leastAngle);
        }

        private double CalculateLeastAngleDifference(DateTime time)
        {
            const double degreesPerHour = 360 / 12;
            const double degreesPerMinute = 360 / 60;

            double hour = time.Hour % 12 + time.Minute / 60.0;
            double minute = time.Minute;

            double angleDifference = Math.Abs(degreesPerHour * hour - degreesPerMinute * minute);

            angleDifference = Math.Min(angleDifference, 360 - angleDifference);

            return angleDifference;
        }

        private WatchResponse GetResponse(DateTime time, double leastAngle)
        {
            var result = new WatchResponse
            {
                Watch = new WatchDto
                {
                    Time = time,
                    WatchHands = new WatchHandsDto
                    {
                        LeastAngle = leastAngle
                    }
                },
                CallTimeStamp = DateTime.Now
            };
            return result;
        }


    }
}
