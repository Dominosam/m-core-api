using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.Models.DTOs
{
    public class WatchHandsDto
    {
        public double LeastAngle { get; set; }
        public double GreatestAngle => 360 - LeastAngle;
        public string LeastAngleAsString => $"{LeastAngle}°C";
        public string GreatestAngleAsString => $"{GreatestAngle}°C";
    }
}
