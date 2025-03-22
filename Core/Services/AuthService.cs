using Bakalauras.Core.DTOs;
using Bakalauras.Core.Entities;
using Bakalauras.Core.Repositories.Interfaces;
using Bakalauras.Core.Services.Interfaces;
using Bakalauras.Core.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bakalauras.Core.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMechanicRepository _mechanicRepository;

    public AuthService(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenService jwtTokenService,
        ICustomerRepository customerRepository,
        IMechanicRepository mechanicRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
        _customerRepository = customerRepository;
        _mechanicRepository = mechanicRepository;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerUser)
    {
        var user = new User
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            FirstName = registerUser.FirstName,
            LastName = registerUser.LastName,
            PhoneNumber = registerUser.PhoneNumber,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);
        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                Success = false,
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow,
                User = new UserDto
                {
                    Id = string.Empty,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    PhoneNumber = string.Empty,
                    Roles = new List<string>()
                }
            };
        }

        await _userManager.AddToRoleAsync(user, registerUser.Role);

        // Create customer or mechanic based on role
        if (registerUser.Role == UserRoles.Customer)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                PhoneNumber = registerUser.PhoneNumber
            };
            await _customerRepository.AddAsync(customer);
        }
        else if (registerUser.Role == UserRoles.Mechanic)
        {
            var mechanic = new Mechanic
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Specialization = "General",
                ExperienceLevel = "1"
            };
            await _mechanicRepository.AddAsync(mechanic);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.Email ?? string.Empty, user.Id, roles);
        var refreshToken = _jwtTokenService.CreateRefreshToken();

        // Store the refresh token
        user.RefreshToken = refreshToken;
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginUser)
    {
        var user = await _userManager.FindByEmailAsync(loginUser.Email);
        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow,
                User = new UserDto
                {
                    Id = string.Empty,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    PhoneNumber = string.Empty,
                    Roles = new List<string>()
                }
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, loginUser.Password);
        if (!result)
        {
            return new AuthResponseDto
            {
                Success = false,
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow,
                User = new UserDto
                {
                    Id = string.Empty,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    PhoneNumber = string.Empty,
                    Roles = new List<string>()
                }
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.Email ?? string.Empty, user.Id, roles);
        var refreshToken = _jwtTokenService.CreateRefreshToken();

        // Store the refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Refresh token valid for 7 days
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            }
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshToken)
    {
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(refreshToken.RefreshToken);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return new AuthResponseDto
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow,
                User = new UserDto
                {
                    Id = string.Empty,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    PhoneNumber = string.Empty,
                    Roles = new List<string>()
                }
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.RefreshToken != refreshToken.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResponseDto
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow,
                User = new UserDto
                {
                    Id = string.Empty,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    PhoneNumber = string.Empty,
                    Roles = new List<string>()
                }
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.Email ?? string.Empty, user.Id, roles);
        var newRefreshToken = _jwtTokenService.CreateRefreshToken();

        // Update the refresh token
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            }
        };
    }

    public async Task ChangeUserRoleAsync(ChangeUserRoleDto changeRole)
    {
        var user = await _userManager.FindByIdAsync(changeRole.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {changeRole.UserId} not found");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, changeRole.NewRole);
    }

    public async Task<UserDto> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return null!;
        }

        var roles = await _userManager.GetRolesAsync(user);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Roles = roles.ToList()
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            });
        }

        return userDtos;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            FirstName = c.User.FirstName,
            LastName = c.User.LastName,
            Email = c.User.Email ?? string.Empty,
            PhoneNumber = c.PhoneNumber,
            UserId = c.UserId
        });
    }

    public async Task<IEnumerable<MechanicDto>> GetAllMechanicsAsync()
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

    public async Task<CustomerDto> GetCustomerByIdAsync(string userId)
    {
        var customer = await _customerRepository.GetByUserIdAsync(userId);
        if (customer == null)
        {
            return null!;
        }

        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.User.FirstName,
            LastName = customer.User.LastName,
            Email = customer.User.Email ?? string.Empty,
            PhoneNumber = customer.PhoneNumber,
            UserId = customer.UserId
        };
    }

    public async Task<MechanicDto> GetMechanicByIdAsync(string userId)
    {
        var mechanic = await _mechanicRepository.GetByUserIdAsync(userId);
        if (mechanic == null)
        {
            return null!;
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

    public async Task<bool> UpdateUserAsync(string id, UpdateUserDto updateDto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        user.FirstName = updateDto.FirstName;
        user.LastName = updateDto.LastName;
        user.PhoneNumber = updateDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<MechanicDto> UpdateMechanicAsync(string userId, UpdateMechanicDto updateMechanic)
    {
        var mechanic = await _mechanicRepository.GetByUserIdAsync(userId);
        if (mechanic == null)
        {
            return null!;
        }

        mechanic.FirstName = updateMechanic.FirstName;
        mechanic.LastName = updateMechanic.LastName;
        mechanic.PhoneNumber = updateMechanic.PhoneNumber;
        mechanic.Specialization = updateMechanic.Specialization;
        mechanic.ExperienceLevel = updateMechanic.ExperienceLevel;

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

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<UserDto> GetUserProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new UserDto
            {
                Id = string.Empty,
                Email = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                PhoneNumber = string.Empty,
                Roles = new List<string>()
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var isCustomer = roles.Contains(UserRoles.Customer);
        var isMechanic = roles.Contains(UserRoles.Mechanic);

        if (isCustomer)
        {
            var customer = await _customerRepository.GetByUserIdAsync(userId);
            if (customer != null)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    Roles = roles.ToList()
                };
            }
        }
        else if (isMechanic)
        {
            var mechanic = await _mechanicRepository.GetByUserIdAsync(userId);
            if (mechanic != null)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                };
            }
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Roles = roles.ToList()
        };
    }
}
