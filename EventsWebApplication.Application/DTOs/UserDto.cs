namespace EventsWebApplication.Application.DTOs
{
    public class User
    {
        private User(Guid id, string name, string surname, DateOnly birthdate, string email, string passwordHash)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthdate;
            PasswordHash = passwordHash;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Surname { get; }

        public string Email { get; }

        public DateOnly BirthDate { get; }
        public string PasswordHash { get; }

        public static User Create(Guid id, string name, string surname, DateOnly birthdate, string email, string passwordHash)
        {
            return new User(id, name,surname,birthdate, email, passwordHash);
        }

    }
}
