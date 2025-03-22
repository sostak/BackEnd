using Bakalauras.Core.DTOs;
using Bakalauras.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakalauras.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MechanicController : ControllerBase
{
    private readonly IMechanicService _mechanicService;

    public MechanicController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MechanicDto>>> GetAll()
    {
        var mechanics = await _mechanicService.GetAllAsync();
        return Ok(mechanics);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MechanicDto>> GetById(string id)
    {
        var mechanic = await _mechanicService.GetByIdAsync(id);
        if (mechanic == null)
        {
            return NotFound();
        }
        return Ok(mechanic);
    }

    [HttpPost]
    public async Task<ActionResult<MechanicDto>> Create(CreateMechanicDto createMechanicDto)
    {
        var mechanic = await _mechanicService.CreateAsync(createMechanicDto);
        return CreatedAtAction(nameof(GetById), new { id = mechanic.Id }, mechanic);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MechanicDto>> Update(string id, UpdateMechanicDto updateMechanicDto)
    {
        try
        {
            var mechanic = await _mechanicService.UpdateAsync(id, updateMechanicDto);
            return Ok(mechanic);
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
            await _mechanicService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
} 