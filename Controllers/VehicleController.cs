using Bakalauras.Core.DTOs;
using Bakalauras.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakalauras.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetById(string id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create(CreateVehicleDto createVehicleDto)
        {
            var vehicle = await _vehicleService.CreateAsync(createVehicleDto);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> Update(string id, UpdateVehicleDto updateVehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateAsync(id, updateVehicleDto);
                return Ok(vehicle);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _vehicleService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
} 