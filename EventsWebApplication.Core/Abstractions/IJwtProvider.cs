using EventsWebApplication.Core.Entities;
using System.Security.Claims;

namespace EventsWebApplication.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateRefreshToken();
        string GenerateToken(UserEntity user);
        ClaimsPrincipal? GetClaimsPrincipal(string token);
    }
}