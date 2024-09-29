

namespace EventsWebApplication.Core.Models
{
    public class User
    {
        private User(Guid id, string userName, string email, string passwordHash)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public Guid Id { get; }

        public string UserName { get; }

        public string Email { get; }

        public string PasswordHash { get; }



        public static User Create(Guid id, string userName, string email, string passwordHash)
        {
            return new User(id, userName, email, passwordHash);
        }

    }
}
