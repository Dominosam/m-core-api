using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Inventory.Models.Requests
{
    public class RemoveQuantityRequestModel
    {
        public string? SKU { get; set; }
        public int Quantity { get; set; }
    }
}
