using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Core.Inventory.Models.DTOs
{
    public class InventoryItemDto
    {
        public string? Id { get; set; }
        public string? SKU { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}
