using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;
using Bakalauras.Core.Models;
using Bakalauras.Core.Repositories.Interfaces;
using Bakalauras.Core.Services.Interfaces;

namespace Bakalauras.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<PaginatedResponse<CustomerDto>> GetAllPaginatedAsync(int pageNumber = 1, int pageSize = 10)
    {
        var customers = await _customerRepository.GetAllAsync();
        var totalCount = customers.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        var items = customers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToDto);

        return new PaginatedResponse<CustomerDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }

    public async Task<CustomerDto?> GetByIdAsync(string id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = createCustomerDto.PhoneNumber,
            UserId = createCustomerDto.UserId
        };

        await _customerRepository.AddAsync(customer);
        return MapToDto(customer);
    }

    public async Task<CustomerDto?> UpdateAsync(string id, UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return null;

        customer.PhoneNumber = updateCustomerDto.PhoneNumber;

        await _customerRepository.UpdateAsync(customer);
        return MapToDto(customer);
    }

    public async Task DeleteAsync(string id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer != null)
        {
            await _customerRepository.DeleteAsync(customer);
        }
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            UserId = customer.UserId,
            PhoneNumber = customer.PhoneNumber,
            FirstName = customer.User.FirstName,
            LastName = customer.User.LastName,
            Email = customer.User.Email ?? string.Empty
        };
    }
} 