namespace EventsWebApplication.Core.Models
{
    public class Member
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public DateOnly BirthDate { get; set; }

        public DateOnly RegistrationDate { get; set; }

        public string Email { get; set; } = string.Empty;


    }
}
