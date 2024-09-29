using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<bool> AlreadyExist(Guid userId);
        Task<UserEntity?> GetByEmail(string email);
        Task<HashSet<Permission>> GetUserPermissions(Guid UserId);
    }
}