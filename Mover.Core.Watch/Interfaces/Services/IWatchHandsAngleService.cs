using Mover.Core.Watch.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.Interfaces.Services
{
    public interface IWatchHandsAngleService
    {
        double CalculateLeastAngleFromTime(DateTime time);
        double CalculateLeastAngleFromCurrentTime();
        WatchResponse GetWatchFromTime(DateTime time);
        WatchResponse GetWatchFromCurrentTime();
    }
}
