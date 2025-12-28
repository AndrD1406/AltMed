using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.BusinessLogic.Models;

public class CommentCreateDto
{
    [Required]
    public Guid PublicationId { get; set; }

    [Required]
    public Guid AuthorId { get; set; }

    public string? Content { get; set; }
    public string? Image64 { get; set; }
}
