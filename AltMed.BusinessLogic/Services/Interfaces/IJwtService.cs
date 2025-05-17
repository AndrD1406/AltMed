using AltMed.BusinessLogic.Dtos.Identity;
using AltMed.DataAccess.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Services.Interfaces;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
}