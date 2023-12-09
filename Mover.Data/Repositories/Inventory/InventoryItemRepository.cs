using MongoDB.Driver;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Inventory.Models;
using Mover.Data.Repositories.Inventory.Models.Enums;
using Redis.OM;
using Redis.OM.Searching;

namespace Mover.Data.Repositories.Inventory
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly IMongoCollection<InventoryItem> _inventoryItems;

        public InventoryItemRepository(IMongoDbContext mongoDbContext)
        {
            var database = mongoDbContext.GetDatabase();
            _inventoryItems = database.GetCollection<InventoryItem>("InventoryItems");
        }

        public InventoryItemRepository(IMongoDatabase mongoDatabase)
        {
            _inventoryItems = mongoDatabase.GetCollection<InventoryItem>("InventoryItems");
        }

        public async Task<InventoryItemAction> UpsertAsync(InventoryItem inventoryItem)
        {
            var filter = Builders<InventoryItem>.Filter.Eq(x => x.SKU, inventoryItem.SKU);
            var existingInventoryItem = await _inventoryItems.Find(filter).FirstOrDefaultAsync();

            if (existingInventoryItem != null)
            {
                if(existingInventoryItem.Description != inventoryItem.Description)
                {
                    return InventoryItemAction.Failed;
                }

                existingInventoryItem.Quantity += inventoryItem.Quantity;
                await _inventoryItems.ReplaceOneAsync(filter, existingInventoryItem);
                return InventoryItemAction.AddedQuantity;
            }
            else
            {
                await _inventoryItems.InsertOneAsync(inventoryItem);
                return InventoryItemAction.Inserted;
            }
        }

        public async Task<InventoryItemAction> RemoveQuantity(string sku, int quantity)
        {
            var filter = Builders<InventoryItem>.Filter.Eq(x => x.SKU, sku);
            var existingInventoryItem = await _inventoryItems.Find(filter).FirstOrDefaultAsync();

            if (existingInventoryItem != null)
            {
                var newQuantity = existingInventoryItem.Quantity - quantity;
                if (newQuantity < 0)
                {
                    return InventoryItemAction.Failed;
                }
                else
                {
                    existingInventoryItem.Quantity -= quantity;
                    await _inventoryItems.ReplaceOneAsync(filter, existingInventoryItem);
                    return InventoryItemAction.RemovedQuantity;
                }
            }

            return InventoryItemAction.Failed;
        }

        public InventoryItem? GetBySKU(string sku)
        {
            var filter = Builders<InventoryItem>.Filter.Eq(x => x.SKU, sku);
            return _inventoryItems.Find(filter).FirstOrDefault();
        }

        public IEnumerable<InventoryItem> GetAll() => _inventoryItems.Find(_ => true).ToList();
    }
}

//namespace Mover.Data.Repositories.Inventory
//{
//    public class InventoryItemRepository : IInventoryItemRepository
//    {
//        private readonly RedisCollection<InventoryItem> _inventoryItems;

//        public InventoryItemRepository(RedisConnectionProvider provider)
//        {
//            _inventoryItems = (RedisCollection<InventoryItem>)provider.RedisCollection<InventoryItem>();
//        }

//        public async Task<InventoryItemAction> UpsertAsync(InventoryItem inventoryItem)
//        {
//            var itemSku = inventoryItem.SKU;

//            var existingInventoryItem = _inventoryItems.FirstOrDefault(item => item.SKU == itemSku);

//            if (existingInventoryItem != null)
//            {
//                existingInventoryItem.Quantity += inventoryItem.Quantity;
//                await _inventoryItems.SaveAsync();
//                return InventoryItemAction.AddedQuantity;
//            }
//            else
//            {
//                await _inventoryItems.InsertAsync(inventoryItem);
//                return InventoryItemAction.Inserted;
//            }
//        }

//        public async Task<InventoryItemAction> RemoveQuantity(string sku, int quantity)
//        {
//            var existingInventoryItem = _inventoryItems.FirstOrDefault(item => item.SKU == sku);

//            if (existingInventoryItem != null)
//            {
//                var newQuantity = existingInventoryItem.Quantity - quantity;
//                if(newQuantity < 0)
//                {
//                    return InventoryItemAction.Failed;
//                }
//                else
//                {
//                    existingInventoryItem.Quantity -= quantity;
//                    await _inventoryItems.SaveAsync();
//                    return InventoryItemAction.RemovedQuantity;
//                }
//            }

//            return InventoryItemAction.Failed;
//        }

//        public InventoryItem? GetBySKU(string sku) => _inventoryItems.FirstOrDefault(item => item.SKU == sku);
//        public IEnumerable<InventoryItem> GetAll() => _inventoryItems.ToList();
//    }
//}
