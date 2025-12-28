using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.BusinessLogic.Models;

public class CommentDto
{
    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Image64 { get; set; } 

    public string Content {  get; set; }

    public Guid AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorLogo { get; set; }
}
