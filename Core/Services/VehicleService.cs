using Bakalauras.Core.Interfaces;
using Bakalauras.Entities;
using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;
using Bakalauras.Core.Repositories.Interfaces;
using Bakalauras.Core.Services.Interfaces;

namespace Bakalauras.Core.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                LicensePlate = v.LicensePlate,
                CustomerId = v.CustomerId
            });
        }

        public async Task<VehicleDto?> GetByIdAsync(string id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                CustomerId = vehicle.CustomerId
            };
        }

        public async Task<VehicleDto> CreateAsync(CreateVehicleDto createVehicleDto)
        {
            var vehicle = new Bakalauras.Core.Entities.Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Make = createVehicleDto.Make,
                Model = createVehicleDto.Model,
                Year = createVehicleDto.Year,
                LicensePlate = createVehicleDto.LicensePlate,
                CustomerId = createVehicleDto.CustomerId
            };

            await _vehicleRepository.AddAsync(vehicle);

            return new VehicleDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                CustomerId = vehicle.CustomerId
            };
        }

        public async Task<VehicleDto> UpdateAsync(string id, UpdateVehicleDto updateVehicleDto)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {id} not found");
            }

            vehicle.Make = updateVehicleDto.Make;
            vehicle.Model = updateVehicleDto.Model;
            vehicle.Year = updateVehicleDto.Year;
            vehicle.LicensePlate = updateVehicleDto.LicensePlate;

            await _vehicleRepository.UpdateAsync(vehicle);

            return new VehicleDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                CustomerId = vehicle.CustomerId
            };
        }

        public async Task DeleteAsync(string id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {id} not found");
            }

            await _vehicleRepository.DeleteAsync(vehicle);
        }
    }
} 