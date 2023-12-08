using Mover.Core.Watch.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.Models.Response
{
    public class WatchResponse
    {
        public WatchDto? Watch { get; set; }
        public DateTime CallTimeStamp { get; set; }
    }
}
