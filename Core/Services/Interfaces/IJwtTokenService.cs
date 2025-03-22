using System.Security.Claims;

namespace Bakalauras.Core.Services.Interfaces;

public interface IJwtTokenService
{
    string CreateAccessToken(string email, string userId, IEnumerable<string> userRoles);
    string CreateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
} 