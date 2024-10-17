
namespace EventsWebApplication.Application.Users.Registration
{
    public class UserRegistrationResponse
    {
        public string JwtToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}
