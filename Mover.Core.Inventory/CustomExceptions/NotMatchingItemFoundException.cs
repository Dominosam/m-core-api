using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Inventory.CustomExceptions
{
    public class NotMatchingItemFoundException : Exception
    {
        public NotMatchingItemFoundException(string message) : base(message)
        {
        }
    }
}
