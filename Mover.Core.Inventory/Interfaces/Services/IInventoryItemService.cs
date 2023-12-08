using Mover.Core.Inventory.Models.DTOs;
using Mover.Data.Repositories.Inventory.Models.Enums;

namespace Mover.Core.Inventory.Interfaces.Services
{
    public interface IInventoryItemService
    {
        Task<InventoryItemAction> UpsertInventoryItem(InventoryItemDto inventoryItem);
        InventoryItemDto? GetInventoryItemBySKU(string sku);
        Task<InventoryItemAction> RemoveInventoryItemQuantity(string sku, int quantity);
        IEnumerable<InventoryItemDto> GetAllInventoryItems();
    }
}
