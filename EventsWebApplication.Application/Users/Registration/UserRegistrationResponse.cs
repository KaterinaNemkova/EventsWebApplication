
namespace EventsWebApplication.Application.Users.Registration
{
    public class UserRegistrationResponse
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }=string.Empty;
        
    }
}
