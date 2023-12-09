using Mover.Core.Inventory.Models.DTOs;
using Mover.Data.Repositories.Inventory.Models.Enums;

namespace Mover.Core.Inventory.Interfaces.Services
{
    public interface IInventoryItemService
    {
        Task<string> UpsertInventoryItem(InventoryItemDto inventoryItem);
        InventoryItemDto? GetInventoryItemBySKU(string sku);
        Task RemoveInventoryItemQuantity(string sku, int quantity);
        IEnumerable<InventoryItemDto> GetAllInventoryItems();
    }
}
