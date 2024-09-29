using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.Application.Services
{
    public interface IPermissionService
    {
        Task<HashSet<Permission>> GetPermissionsAsync(Guid UserId);
    }
}