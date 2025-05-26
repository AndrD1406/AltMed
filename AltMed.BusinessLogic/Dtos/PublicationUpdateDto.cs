using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Dtos;

public class PublicationUpdateDto
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? Image64 { get; set; }
}
