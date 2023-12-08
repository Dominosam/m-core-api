using Mover.Data.Repositories.Watch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.Models.DTOs
{
    public class WatchDto
    {
        public WatchHandsDto WatchHands { get; set; }
        public DateTime Time { get; set; }

        public string TimeAsString => Time.ToString("HH:mm");
    }
}
