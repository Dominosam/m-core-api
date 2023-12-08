
namespace Mover.Data.Interfaces
{
    public interface IWatchHandsRepository
    {
        void SaveResponse(DateTime time, double leastAngle);
        double? GetNewestLeastAngle();
    }
}
