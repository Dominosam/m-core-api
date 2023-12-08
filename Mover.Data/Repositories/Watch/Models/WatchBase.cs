using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Data.Repositories.Watch.Models
{
    public class WatchBase
    {
        public string WatchId { get; set; }
        public WatchHands WatchHands { get; set; }
        public DateTime Time { get; set; }

        public WatchBase()
        {
            WatchId = Guid.NewGuid().ToString();
        }
    }
}
