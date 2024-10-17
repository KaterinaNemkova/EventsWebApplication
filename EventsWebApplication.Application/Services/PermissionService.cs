using EventsWebApplication.Core.Enums;
using EventsWebApplication.DataAccess.Repositories;


namespace EventsWebApplication.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUserRepository _userRepository;

        public PermissionService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<HashSet<Permission>> GetPermissionsAsync(Guid UserId)
        {
            return _userRepository.GetUserPermissions(UserId);
        }
    }
}
