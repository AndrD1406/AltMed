using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.BusinessLogic.Models;

public class ProfileDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public string Logo { get; set; } = default!;
    public List<Guid> PublicationIds { get; set; } = new();
}