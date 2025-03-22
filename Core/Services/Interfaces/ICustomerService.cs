using Bakalauras.Core.DTOs;
using Bakalauras.Core.Models;

namespace Bakalauras.Core.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<PaginatedResponse<CustomerDto>> GetAllPaginatedAsync(int pageNumber = 1, int pageSize = 10);
    Task<CustomerDto?> GetByIdAsync(string id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto);
    Task<CustomerDto?> UpdateAsync(string id, UpdateCustomerDto updateCustomerDto);
    Task DeleteAsync(string id);
} 