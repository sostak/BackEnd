using AutoMapper;
using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;

namespace Bakalauras.Core.Mapping;

public class MechanicProfile : Profile
{
    public MechanicProfile()
    {
        CreateMap<Mechanic, MechanicDto>();
        CreateMap<CreateMechanicDto, Mechanic>();
        CreateMap<UpdateMechanicDto, Mechanic>();
    }
} 