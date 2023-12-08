using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Data.Repositories.Watch.Models
{
    public class WatchHands
    {
        public string WatchHandsId { get; set; }
        public double LeastAngle { get; set; }
        public double GreatestAngle { get; set; }

        public WatchHands()
        {
            WatchHandsId = Guid.NewGuid().ToString();
        }
    }
}
