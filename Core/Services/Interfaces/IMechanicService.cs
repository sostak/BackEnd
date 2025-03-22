using Bakalauras.Core.DTOs;

namespace Bakalauras.Core.Services.Interfaces;

public interface IMechanicService
{
    Task<IEnumerable<MechanicDto>> GetAllAsync();
    Task<MechanicDto?> GetByIdAsync(string id);
    Task<MechanicDto> CreateAsync(CreateMechanicDto createMechanicDto);
    Task<MechanicDto> UpdateAsync(string id, UpdateMechanicDto updateMechanicDto);
    Task DeleteAsync(string id);
} 