using Microsoft.AspNetCore.Authorization;

namespace AltWirePoint.Common.PermissionModule.PolicyClasses;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(permission.ToString())
    {
        
    }
}
