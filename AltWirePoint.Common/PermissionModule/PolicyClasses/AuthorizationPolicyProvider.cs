using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AltWirePoint.Common.PermissionModule.PolicyClasses;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions options;

    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options) =>
        this.options = options.Value;

    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        return await base.GetPolicyAsync(policyName)
               ?? new AuthorizationPolicyBuilder()
                   .AddRequirements(new PermissionRequirement(policyName))
                   .Build();
    }
}
