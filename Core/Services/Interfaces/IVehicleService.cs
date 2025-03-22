using Bakalauras.Core.DTOs;

namespace Bakalauras.Core.Services.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetAllAsync();
    Task<VehicleDto?> GetByIdAsync(string id);
    Task<VehicleDto> CreateAsync(CreateVehicleDto createVehicleDto);
    Task<VehicleDto> UpdateAsync(string id, UpdateVehicleDto updateVehicleDto);
    Task DeleteAsync(string id);
} 