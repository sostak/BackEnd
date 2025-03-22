using Bakalauras.Core.DTOs;

namespace Bakalauras.Core.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerUser);
    Task<AuthResponseDto> LoginAsync(LoginUserDto loginUser);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshToken);
    Task ChangeUserRoleAsync(ChangeUserRoleDto changeRole);
    Task<UserDto> GetUserByIdAsync(string id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<IEnumerable<MechanicDto>> GetAllMechanicsAsync();
    Task<CustomerDto> GetCustomerByIdAsync(string userId);
    Task<MechanicDto> GetMechanicByIdAsync(string userId);
    Task<bool> UpdateUserAsync(string id, UpdateUserDto updateDto);
    Task<MechanicDto> UpdateMechanicAsync(string userId, UpdateMechanicDto updateMechanic);
    Task<bool> DeleteUserAsync(string id);
} 