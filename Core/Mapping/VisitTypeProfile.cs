using AutoMapper;
using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;

namespace Bakalauras.Core.Mapping;

public class VisitTypeProfile : Profile
{
    public VisitTypeProfile()
    {
        CreateMap<VisitType, VisitTypeDto>();
        CreateMap<CreateVisitTypeDto, VisitType>();
        CreateMap<UpdateVisitTypeDto, VisitType>();
    }
} 