using System.Security.Claims;

namespace AuthenticationModule.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(IEnumerable<Claim> claims, int expirationMinutes = 60);
    ClaimsPrincipal? ValidateToken(string token);
    string GenerateRefreshToken();
}

