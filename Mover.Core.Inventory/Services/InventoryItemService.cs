using AutoMapper;
using Mover.Core.Inventory.CustomExceptions;
using Mover.Core.Inventory.Interfaces.Services;
using Mover.Core.Inventory.Models.DTOs;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Inventory.Models;
using Mover.Data.Repositories.Inventory.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mover.Core.Inventory.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IMapper _mapper;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository, IMapper mapper)
        {
            _inventoryItemRepository = inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> UpsertInventoryItem(InventoryItemDto inventoryItem)
        {
            var action = await _inventoryItemRepository.UpsertAsync(_mapper.Map<InventoryItem>(inventoryItem));

            switch (action)
            {
                case InventoryItemAction.AddedQuantity:
                    return $"Inventory item updated successfully: SKU - {inventoryItem.SKU}, added quantity by: {inventoryItem.Quantity}";

                case InventoryItemAction.Inserted:
                default:
                    return $"Inventory item created successfully: SKU - {inventoryItem.SKU}";
            }
        }
        public async Task RemoveInventoryItemQuantity(string sku, int quantity)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new MissingSkuException("SKU is required.");
            }

            if (quantity <= 0)
            {
                throw new InsufficientQuantityException("Quantity must be greater than zero.");
            }

            var action = await _inventoryItemRepository.RemoveQuantity(sku, quantity);

            if (action != InventoryItemAction.RemovedQuantity)
            {
                throw new ValidationException($"Failed to remove quantity - {quantity} - from inventory item: SKU - {sku}");
            }
        }

        public InventoryItemDto? GetInventoryItemBySKU(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                var errorMessage = $"SKU is required.";
                throw new MissingSkuException(errorMessage);
            }

            var itemDetails = _inventoryItemRepository.GetBySKU(sku);
            if (itemDetails == null)
            {
                var notFoundMessage = $"Inventory item not found: SKU - {sku}";
                throw new InventoryItemNotFoundException(notFoundMessage);
            }

            return _mapper.Map<InventoryItemDto>(itemDetails);

        }

        public IEnumerable<InventoryItemDto> GetAllInventoryItems()
        {
            var allItems = _inventoryItemRepository.GetAll();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(allItems);
        }
    }
}
