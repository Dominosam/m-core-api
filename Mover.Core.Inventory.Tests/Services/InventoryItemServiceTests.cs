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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Fact]
        public async Task RemoveInventoryItemQuantity_ShouldRemoveQuantity_WhenActionIsRemovedQuantity()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var sku = "123";
            var quantity = 5;

            _mockRepository.Setup(r => r.RemoveQuantity(sku, quantity)).ReturnsAsync(InventoryItemAction.RemovedQuantity);

            // Act
            await service.RemoveInventoryItemQuantity(sku, quantity);

            // Assert
            // No exception is thrown, meaning the operation completed successfully.
        }

        [Fact]
        public async Task RemoveInventoryItemQuantity_ShouldThrowException_WhenActionIsNotRemovedQuantity()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var sku = "123";
            var quantity = 5;

            _mockRepository.Setup(r => r.RemoveQuantity(sku, quantity)).ReturnsAsync(InventoryItemAction.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => service.RemoveInventoryItemQuantity(sku, quantity));
        }

        [Theory]
        [InlineData(null, 5)]
        [InlineData("", 5)]
        [InlineData("   ", 5)]
        public async Task RemoveInventoryItemQuantity_ShouldThrowException_WhenSkuIsInvalid(string sku, int quantity)
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<MissingSkuException>(() => service.RemoveInventoryItemQuantity(sku, quantity));
        }

        [Fact]
        public async Task RemoveInventoryItemQuantity_ShouldThrowException_WhenQuantityIsInvalid()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var sku = "123";
            var invalidQuantity = 0;

            // Act & Assert
            await Assert.ThrowsAsync<InsufficientQuantityException>(() => service.RemoveInventoryItemQuantity(sku, invalidQuantity));
        }

        [Fact]
        public void GetInventoryItemBySKU_ShouldReturnItemDto_WhenItemExists()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var sku = "123";
            var mockInventoryItem = new InventoryItem { SKU = sku, Quantity = 10 };
            var expectedDto = new InventoryItemDto { SKU = sku, Quantity = 10 };

            _mockRepository.Setup(r => r.GetBySKU(sku)).Returns(mockInventoryItem);
            _mockMapper.Setup(m => m.Map<InventoryItemDto>(mockInventoryItem)).Returns(expectedDto);

            // Act
            var result = service.GetInventoryItemBySKU(sku);

            // Assert
            Assert.Equal(expectedDto, result);
        }


        [Fact]
        public void GetInventoryItemBySKU_ShouldThrowException_WhenSKUIsNull()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            string sku = null;

            // Act & Assert
            var exception = Assert.Throws<MissingSkuException>(() => service.GetInventoryItemBySKU(sku));
            Assert.Equal("SKU is required.", exception.Message);
        }

        [Fact]
        public void GetInventoryItemBySKU_ShouldThrowException_WhenItemNotFound()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var sku = "123";

            _mockRepository.Setup(r => r.GetBySKU(sku)).Returns((InventoryItem)null);

            // Act & Assert
            var exception = Assert.Throws<InventoryItemNotFoundException>(() => service.GetInventoryItemBySKU(sku));
            Assert.Equal($"Inventory item not found: SKU - {sku}", exception.Message);
        }

        [Fact]
        public void GetAllInventoryItems_ShouldReturnListOfItemDtos()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);
            var mockInventoryItems = new List<InventoryItem>
            {
                new InventoryItem { SKU = "123", Quantity = 10 },
                new InventoryItem { SKU = "456", Quantity = 5 }
            };
            var expectedDtos = mockInventoryItems.Select(item => new InventoryItemDto { SKU = item.SKU, Quantity = item.Quantity });

            _mockRepository.Setup(r => r.GetAll()).Returns(mockInventoryItems);
            _mockMapper.Setup(m => m.Map<IEnumerable<InventoryItemDto>>(mockInventoryItems)).Returns(expectedDtos);

            // Act
            var result = service.GetAllInventoryItems();

            // Assert

            Assert.Equal(expectedDtos.ToList()[0].SKU, result.ToList()[0].SKU);
            Assert.Equal(expectedDtos.ToList()[0].Quantity, result.ToList()[0].Quantity);
            Assert.Equal(expectedDtos.ToList()[1].SKU, result.ToList()[1].SKU);
            Assert.Equal(expectedDtos.ToList()[1].Quantity, result.ToList()[1].Quantity);
        }

        [Fact]
        public void GetAllInventoryItems_ShouldThrowException_WhenRepositoryReturnsNull()
        {
            // Arrange
            var service = new InventoryItemService(_mockRepository.Object, _mockMapper.Object);

            _mockRepository.Setup(r => r.GetAll()).Returns((List<InventoryItem>)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => service.GetAllInventoryItems());
        }
    }
}
