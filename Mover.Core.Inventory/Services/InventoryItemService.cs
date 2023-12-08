using AutoMapper;
using Mover.Core.Inventory.Interfaces.Services;
using Mover.Core.Inventory.Models.DTOs;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Inventory.Models;
using Mover.Data.Repositories.Inventory.Models.Enums;

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

        public async Task<InventoryItemAction> UpsertInventoryItem(InventoryItemDto inventoryItem)
        {
            return await _inventoryItemRepository.UpsertAsync(_mapper.Map<InventoryItem>(inventoryItem));
        }
        public async Task<InventoryItemAction> RemoveInventoryItemQuantity(string sku, int quantity)
        {
            return await _inventoryItemRepository.RemoveQuantity(sku, quantity);
        }

        public InventoryItemDto? GetInventoryItemBySKU(string sku)
        {
            var inventoryItem = _inventoryItemRepository.GetBySKU(sku);
            return _mapper.Map<InventoryItemDto>(inventoryItem);
        }

        public IEnumerable<InventoryItemDto> GetAllInventoryItems()
        {
            var allItems = _inventoryItemRepository.GetAll();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(allItems);
        }
    }
}
