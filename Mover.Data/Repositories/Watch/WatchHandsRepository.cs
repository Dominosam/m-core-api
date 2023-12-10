using Mover.Data.Contexts;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Watch.Models;
using Newtonsoft.Json;
using Serilog;

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

        public bool SaveWatchResponse(DateTime time, double leastAngle)
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

            return SaveResponse(watch);
        }

        private bool SaveResponse(WatchBase response)
        {
            try
            {
                var database = _redisContext.GetDatabase();
                var serializedResponse = JsonConvert.SerializeObject(response);

                database.StringSet(_defaultRepositoryKey, serializedResponse);
                return true; 
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false; // Indicate failure
            }
        }
    }
}