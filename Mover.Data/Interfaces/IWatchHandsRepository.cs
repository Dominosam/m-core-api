
namespace Mover.Data.Interfaces
{
    public interface IWatchHandsRepository
    {
        bool SaveWatchResponse(DateTime time, double leastAngle);
    }
}
