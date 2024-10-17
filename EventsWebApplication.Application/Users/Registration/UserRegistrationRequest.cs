
namespace EventsWebApplication.Application.Users.Registration
{
    public class UserRegistrationRequest
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required DateOnly BirthDate { get; set; }
        public required string Password { get; set; }
    }
}
