using AltWirePoint.DataAccess.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.DataAccess.Models;

public class Publication : IKeyedEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? Image64 { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ApplicationUser))]
    public Guid AuthorId { get; set; }

    public virtual ApplicationUser? Author { get; set; }

    public virtual List<Like>? Likes { get; set; }
    public Guid? ParentId { get; set; } = null;
    [ForeignKey(nameof(ParentId))]

    public virtual Publication? Parent { get; set; }

    public virtual List<Publication> Comments { get; set; } = new List<Publication>();
}
