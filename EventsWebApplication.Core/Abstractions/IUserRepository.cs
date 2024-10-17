using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;


namespace EventsWebApplication.DataAccess.Repositories
{
    public interface IUserRepository: IRepository<UserEntity>
    {
        Task Create(UserEntity user);
        Task<bool> AlreadyExist(Guid userId);
        Task<UserEntity?> GetByEmail(string email);
        Task<HashSet<Permission>> GetUserPermissions(Guid UserId);
     
    }
}