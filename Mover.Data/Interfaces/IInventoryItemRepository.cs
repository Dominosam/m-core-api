using Mover.Data.Repositories.Inventory.Models;
using Mover.Data.Repositories.Inventory.Models.Enums;

namespace Mover.Data.Interfaces
{
    public interface IInventoryItemRepository
    {
        Task<InventoryItemAction> UpsertAsync(InventoryItem inventoryItem);
        Task<InventoryItemAction> RemoveQuantity(string sku, int quantity);
        InventoryItem? GetBySKU(string sku);
        IEnumerable<InventoryItem> GetAll();
    }
}
