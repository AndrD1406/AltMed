using AltWirePoint.Common.PermissionManagement;
using Microsoft.AspNetCore.Authorization;

namespace AltWirePoint.Common.PermissionModule.PolicyClasses;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permission) : base(permission.ToString())
    {
        
    }
}
