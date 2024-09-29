using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}