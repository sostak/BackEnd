using AutoMapper;
using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;

namespace Bakalauras.Core.Mapping;

public class InventoryItemProfile : Profile
{
    public InventoryItemProfile()
    {
        CreateMap<InventoryItem, InventoryItemDto>();
        CreateMap<CreateInventoryItemDto, InventoryItem>();
        CreateMap<UpdateInventoryItemDto, InventoryItem>();
    }
} 