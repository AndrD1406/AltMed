using AltWirePoint.BusinessLogic.Models.Identity;
using AltWirePoint.DataAccess.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.BusinessLogic.Services.Interfaces;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
}