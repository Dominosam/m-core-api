using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Watch.CustomExceptions
{
    public class CalculateLeastAngleException : Exception
    {
        public CalculateLeastAngleException(string message) : base(message)
        {
        }
    }
}
