using AutoMapper;
using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;

namespace Bakalauras.Core.Mapping;

public class InventoryOperationProfile : Profile
{
    public InventoryOperationProfile()
    {
        CreateMap<InventoryOperation, InventoryOperationDto>();
        CreateMap<CreateInventoryOperationDto, InventoryOperation>();
        CreateMap<UpdateInventoryOperationDto, InventoryOperation>();
    }
} 