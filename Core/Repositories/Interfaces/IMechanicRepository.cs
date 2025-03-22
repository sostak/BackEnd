using Bakalauras.Core.Entities;

namespace Bakalauras.Core.Repositories.Interfaces;

public interface IMechanicRepository
{
    Task<IEnumerable<Mechanic>> GetAllAsync();
    Task<Mechanic?> GetByIdAsync(string id);
    Task<Mechanic?> GetByUserIdAsync(string userId);
    Task<Mechanic> AddAsync(Mechanic mechanic);
    Task<Mechanic> UpdateAsync(Mechanic mechanic);
    Task DeleteAsync(Mechanic mechanic);
} 