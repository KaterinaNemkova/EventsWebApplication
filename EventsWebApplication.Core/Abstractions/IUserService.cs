

namespace EventsWebApplication.Application.Services
{
    public interface IUserService
    {
        Task<bool> AlreadyExist(Guid userId);
        Task<string> Login(string email, string password);
        Task Register(string name, string email, string password);
    }
}