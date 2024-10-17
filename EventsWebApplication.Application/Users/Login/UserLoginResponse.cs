
namespace EventsWebApplication.Application.Users.Login
{
    public class UserLoginResponse
    {
        public string JwtToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}
