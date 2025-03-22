using Bakalauras.Core.Entities;
using Bakalauras.Core.Repositories.Interfaces;
using Bakalauras.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bakalauras.Core.Repositories;

public class MechanicRepository : IMechanicRepository
{
    private readonly ApplicationDbContext _context;

    public MechanicRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Mechanic>> GetAllAsync()
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .ToListAsync();
    }

    public async Task<Mechanic?> GetByIdAsync(string id)
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mechanic?> GetByUserIdAsync(string userId)
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<Mechanic> AddAsync(Mechanic mechanic)
    {
        await _context.Mechanics.AddAsync(mechanic);
        await _context.SaveChangesAsync();
        return mechanic;
    }

    public async Task<Mechanic> UpdateAsync(Mechanic mechanic)
    {
        _context.Mechanics.Update(mechanic);
        await _context.SaveChangesAsync();
        return mechanic;
    }

    public async Task DeleteAsync(Mechanic mechanic)
    {
        _context.Mechanics.Remove(mechanic);
        await _context.SaveChangesAsync();
    }
} 