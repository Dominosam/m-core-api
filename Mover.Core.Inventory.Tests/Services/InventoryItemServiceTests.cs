using AutoMapper;
using Moq;
using Mover.Core.Inventory.CustomExceptions;
using Mover.Core.Inventory.Models.DTOs;
using Mover.Core.Inventory.Services;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Inventory.Models;
using Mover.Data.Repositories.Inventory.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mover.Core.Inventory.Tests.Services
{
    public class InventoryItemServiceTests
    {
        private readonly Mock<IInventoryItemRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public InventoryItemServiceTests()
        {
            _mockRepository = new Mock<IInventoryItemRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task UpsertInventoryItem_ShouldUpdateQuantity_WhenActionIsAddedQuantity()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var inventoryItemDto = new InventoryItemDto { SKU = "123", Quantity = 5 };
            var inventoryItem = new InventoryItem { SKU = "123", Quantity = 10 };

            _mockMapper.Setup(m => m.Map<InventoryItem>(inventoryItemDto)).Returns(inventoryItem);
            _mockRepository.Setup(r => r.UpsertAsync(inventoryItem)).ReturnsAsync(InventoryItemAction.AddedQuantity);

            // Act
            var result = await service.UpsertInventoryItem(inventoryItemDto);

            // Assert
            Assert.Equal($"Inventory item updated successfully: SKU - {inventoryItemDto.SKU}, added quantity by: {inventoryItemDto.Quantity}", result);
        }

        [Fact]
        public async Task UpsertInventoryItem_ShouldCreateNewItem_WhenActionIsInserted()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var inventoryItemDto = new InventoryItemDto { SKU = "123", Quantity = 5 };
            var inventoryItem = new InventoryItem { SKU = "123", Quantity = 5 };

            _mockMapper.Setup(m => m.Map<InventoryItem>(inventoryItemDto)).Returns(inventoryItem);
            _mockRepository.Setup(r => r.UpsertAsync(inventoryItem)).ReturnsAsync(InventoryItemAction.Inserted);

            // Act
            var result = await service.UpsertInventoryItem(inventoryItemDto);

            // Assert
            Assert.Equal($"Inventory item created successfully: SKU - {inventoryItemDto.SKU}", result);
        }

        [Fact]
        public async Task UpsertInventoryItem_ShouldThrowException_WhenActionIsFailed()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var inventoryItemDto = new InventoryItemDto { SKU = "123", Quantity = 5 };
            var inventoryItem = new InventoryItem { SKU = "123", Quantity = 5 };

            _mockMapper.Setup(m => m.Map<InventoryItem>(inventoryItemDto)).Returns(inventoryItem);
            _mockRepository.Setup(r => r.UpsertAsync(inventoryItem)).ReturnsAsync(InventoryItemAction.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<NotMatchingItemFoundException>(() => service.UpsertInventoryItem(inventoryItemDto));
        }
    }
}
