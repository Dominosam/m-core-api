using Mover.Data.Contexts;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Watch.Models;
using Newtonsoft.Json;

namespace Mover.Data.Repositories.Watch
{
    public class WatchHandsRepository : IWatchHandsRepository
    {
        private readonly IRedisContext _redisContext;
        private readonly string _defaultRepositoryKey = "watch";

        public WatchHandsRepository(IRedisContext redisContext)
        {
            _redisContext = redisContext ?? throw new ArgumentNullException(nameof(redisContext));
        }

        public void SaveResponse(DateTime time, double leastAngle)
        {
            var watchHands = new WatchHands
            {
                LeastAngle = leastAngle,
                GreatestAngle = 360 - leastAngle,
            };

            var watch = new WatchBase
            {
                Time = time,
                WatchHands = watchHands
            };

            SaveResponse(watch);
        }

        public double? GetNewestLeastAngle()
        {
            var database = _redisContext.GetDatabase();
            var serializedResponses = database.ListRange(_defaultRepositoryKey);

            if (serializedResponses.Any())
            {
                var responses = serializedResponses.Select(sr => JsonConvert.DeserializeObject<WatchBase>(sr));
                var newestResponse = responses.OrderByDescending(r => r.Time).FirstOrDefault();

                return newestResponse?.WatchHands.LeastAngle;
            }

            return null;
        }

        private void SaveResponse(WatchBase response)
        {
            var database = _redisContext.GetDatabase();
            var serializedResponse = JsonConvert.SerializeObject(response);

            // Save the serialized response to Redis
            database.StringSet(_defaultRepositoryKey, serializedResponse);
        }
    }
}