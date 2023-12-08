using AutoMapper;
using Mover.Core.Inventory.Models.DTOs;
using Mover.Data.Repositories.Inventory.Models;

namespace Mover.Core.Inventory.Models.Profiles
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            CreateMap<InventoryItemDto, InventoryItem>();
            CreateMap<InventoryItem, InventoryItemDto> ();
        }
    }
}
