using AltWirePoint.BusinessLogic.Models.Identity;
using AltWirePoint.DataAccess.Identity;
using System.Security.Claims;

namespace AltWirePoint.BusinessLogic.Services.Interfaces;

public interface IJwtService
{
    Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
}