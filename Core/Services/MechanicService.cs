using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;
using Bakalauras.Core.Repositories.Interfaces;
using Bakalauras.Core.Services.Interfaces;

namespace Bakalauras.Core.Services;

public class MechanicService : IMechanicService
{
    private readonly IMechanicRepository _mechanicRepository;

    public MechanicService(IMechanicRepository mechanicRepository)
    {
        _mechanicRepository = mechanicRepository;
    }

    public async Task<IEnumerable<MechanicDto>> GetAllAsync()
    {
        var mechanics = await _mechanicRepository.GetAllAsync();
        return mechanics.Select(m => new MechanicDto
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            Email = m.Email,
            PhoneNumber = m.PhoneNumber,
            UserId = m.UserId,
            Specialization = m.Specialization,
            ExperienceLevel = m.ExperienceLevel
        });
    }

    public async Task<MechanicDto?> GetByIdAsync(string id)
    {
        var mechanic = await _mechanicRepository.GetByIdAsync(id);
        if (mechanic == null)
        {
            return null;
        }

        return new MechanicDto
        {
            Id = mechanic.Id,
            FirstName = mechanic.FirstName,
            LastName = mechanic.LastName,
            Email = mechanic.Email,
            PhoneNumber = mechanic.PhoneNumber,
            UserId = mechanic.UserId,
            Specialization = mechanic.Specialization,
            ExperienceLevel = mechanic.ExperienceLevel
        };
    }

    public async Task<MechanicDto> CreateAsync(CreateMechanicDto createMechanicDto)
    {
        var mechanic = new Mechanic
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = createMechanicDto.FirstName,
            LastName = createMechanicDto.LastName,
            Email = createMechanicDto.Email,
            PhoneNumber = createMechanicDto.PhoneNumber,
            UserId = createMechanicDto.UserId,
            Specialization = createMechanicDto.Specialization,
            ExperienceLevel = createMechanicDto.ExperienceLevel
        };

        await _mechanicRepository.AddAsync(mechanic);

        return new MechanicDto
        {
            Id = mechanic.Id,
            FirstName = mechanic.FirstName,
            LastName = mechanic.LastName,
            Email = mechanic.Email,
            PhoneNumber = mechanic.PhoneNumber,
            UserId = mechanic.UserId,
            Specialization = mechanic.Specialization,
            ExperienceLevel = mechanic.ExperienceLevel
        };
    }

    public async Task<MechanicDto> UpdateAsync(string id, UpdateMechanicDto updateMechanicDto)
    {
        var mechanic = await _mechanicRepository.GetByIdAsync(id);
        if (mechanic == null)
        {
            throw new KeyNotFoundException($"Mechanic with ID {id} not found");
        }

        mechanic.FirstName = updateMechanicDto.FirstName;
        mechanic.LastName = updateMechanicDto.LastName;
        mechanic.Email = updateMechanicDto.Email;
        mechanic.PhoneNumber = updateMechanicDto.PhoneNumber;
        mechanic.Specialization = updateMechanicDto.Specialization;
        mechanic.ExperienceLevel = updateMechanicDto.ExperienceLevel;

        await _mechanicRepository.UpdateAsync(mechanic);

        return new MechanicDto
        {
            Id = mechanic.Id,
            FirstName = mechanic.FirstName,
            LastName = mechanic.LastName,
            Email = mechanic.Email,
            PhoneNumber = mechanic.PhoneNumber,
            UserId = mechanic.UserId,
            Specialization = mechanic.Specialization,
            ExperienceLevel = mechanic.ExperienceLevel
        };
    }

    public async Task DeleteAsync(string id)
    {
        var mechanic = await _mechanicRepository.GetByIdAsync(id);
        if (mechanic == null)
        {
            throw new KeyNotFoundException($"Mechanic with ID {id} not found");
        }

        await _mechanicRepository.DeleteAsync(mechanic);
    }
} 