using AltWirePoint.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.DataAccess.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? Name { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpirationDateTime { get; set; }

    public virtual List<Publication>? Publications { get; set; }

    public virtual List<Like>? Likes { get; set; }

    public string? Logo { get; set; }

    public string? Description { get; set; }
}
