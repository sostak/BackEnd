using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bakalauras.Core.Auth;
using Bakalauras.Core.DTOs;
using Bakalauras.Core.Services.Interfaces;

namespace Bakalauras.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterUserDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginUserDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleDto changeRoleDto)
        {
            await _authService.ChangeUserRoleAsync(changeRoleDto);
            return Ok();
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await _authService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("customers")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _authService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("mechanics")]
        public async Task<ActionResult<IEnumerable<MechanicDto>>> GetAllMechanics()
        {
            var mechanics = await _authService.GetAllMechanicsAsync();
            return Ok(mechanics);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("customers/{userId}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(string userId)
        {
            var customer = await _authService.GetCustomerByIdAsync(userId);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("mechanics/{userId}")]
        public async Task<ActionResult<MechanicDto>> GetMechanicById(string userId)
        {
            var mechanic = await _authService.GetMechanicByIdAsync(userId);
            if (mechanic == null)
            {
                return NotFound();
            }
            return Ok(mechanic);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateDto)
        {
            var result = await _authService.UpdateUserAsync(id, updateDto);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("mechanics/{userId}")]
        public async Task<ActionResult<MechanicDto>> UpdateMechanic(string userId, UpdateMechanicDto updateMechanic)
        {
            var mechanic = await _authService.UpdateMechanicAsync(userId, updateMechanic);
            if (mechanic == null)
            {
                return NotFound();
            }
            return Ok(mechanic);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _authService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
