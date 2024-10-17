using EventsWebApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace EventsWebApplication.Infrastructure
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var UserId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId);

            if (UserId is null || !Guid.TryParse(UserId.Value, out var id))
            {
                return;
            }

            using var scope = _scopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            var permissions = await permissionService.GetPermissionsAsync(id);

            if (requirement.Permissions.All(rp => permissions.Contains(rp)))
            {
                context.Succeed(requirement);
            }



        }
    }
}
