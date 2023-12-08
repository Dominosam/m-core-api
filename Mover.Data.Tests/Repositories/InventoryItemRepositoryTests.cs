using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using Mover.Data.Contexts;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Inventory;
using Mover.Data.Repositories.Inventory.Models;
using Mover.Data.Repositories.Inventory.Models.Enums;
using Xunit;

namespace Mover.Data.Tests.Repositories.Inventory
{
    public class InventoryItemRepositoryTests : IDisposable
    {
        private readonly IMongoDatabase _database;
        private readonly InventoryItemRepository _repository;
        private const string CONNECTION_STRING_KEY = "MongoDB";
        private readonly IConfiguration _configuration;

        public InventoryItemRepositoryTests()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("TestDatabase");
            _repository = new InventoryItemRepository(_database);
        }

        [Fact]
        public async Task UpsertAsync_ShouldAddNewInventoryItem_WhenItemDoesNotExist()
        {
            // Arrange
            var inventoryItem = new InventoryItem
            {
                SKU = "TestSKU",
                Quantity = 10
            };

            // Act
            var result = await _repository.UpsertAsync(inventoryItem);

            // Assert
            Assert.Equal(InventoryItemAction.Inserted, result);

            var storedItem = _repository.GetBySKU("TestSKU");
            Assert.NotNull(storedItem);
            Assert.Equal(10, storedItem.Quantity);
        }

        [Fact]
        public async Task UpsertAsync_ShouldUpdateQuantity_WhenItemAlreadyExists()
        {
            // Arrange
            var existingItem = new InventoryItem
            {
                SKU = "TestSKU",
                Quantity = 5
            };

            await _repository.UpsertAsync(existingItem);

            var updatedItem = new InventoryItem
            {
                SKU = "TestSKU",
                Quantity = 10
            };

            // Act
            var result = await _repository.UpsertAsync(updatedItem);

            // Assert
            Assert.Equal(InventoryItemAction.AddedQuantity, result);

            var storedItem = _repository.GetBySKU("TestSKU");
            Assert.NotNull(storedItem);
            Assert.Equal(15, storedItem.Quantity);
        }

        [Fact]
        public async Task RemoveQuantity_ShouldFail_WhenItemDoesNotExist()
        {
            // Act
            var result = await _repository.RemoveQuantity("NonExistentSKU", 5);

            // Assert
            Assert.Equal(InventoryItemAction.Failed, result);
        }

        [Fact]
        public async Task RemoveQuantity_ShouldFail_WhenQuantityIsGreaterThanCurrent()
        {
            // Arrange
            var existingItem = new InventoryItem
            {
                SKU = "TestSKU",
                Quantity = 5
            };

            await _repository.UpsertAsync(existingItem);

            // Act
            var result = await _repository.RemoveQuantity("TestSKU", 10);

            // Assert
            Assert.Equal(InventoryItemAction.Failed, result);

            var storedItem = _repository.GetBySKU("TestSKU");
            Assert.NotNull(storedItem);
            Assert.Equal(5, storedItem.Quantity);
        }

        [Fact]
        public async Task RemoveQuantity_ShouldSucceed_WhenQuantityIsLessThanCurrent()
        {
            // Arrange
            var existingItem = new InventoryItem
            {
                SKU = "TestSKU",
                Quantity = 10
            };

            await _repository.UpsertAsync(existingItem);

            // Act
            var result = await _repository.RemoveQuantity("TestSKU", 5);

            // Assert
            Assert.Equal(InventoryItemAction.RemovedQuantity, result);

            var storedItem = _repository.GetBySKU("TestSKU");
            Assert.NotNull(storedItem);
            Assert.Equal(5, storedItem.Quantity);
        }

        public void Dispose()
        {
            _database.Client.DropDatabase("TestDatabase");
        }
    }
}