using AltWirePoint.Common.PermissionManagement;
using Microsoft.AspNetCore.Authorization;

namespace AltWirePoint.Common.PermissionModule.PolicyClasses;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissionsClaim =
            context.User.Claims.SingleOrDefault(c => c.Type == IdentityResourceClaimsTypes.Permissions);

        if (permissionsClaim?.Value.ThisPermissionIsAllowed(requirement) ?? false)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
