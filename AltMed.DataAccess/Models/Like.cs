using AltMed.DataAccess.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.DataAccess.Models;

public class Like : IKeyedEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(ApplicationUser))]
    public Guid? AuthorId { get; set; }

    public virtual ApplicationUser? Author { get; set; }

    [ForeignKey(nameof(Publication))]
    public Guid PublicationId { get; set; }

    public virtual Publication? Publication { get; set; }

    public bool IsLiked { get; set; } = true;
}
