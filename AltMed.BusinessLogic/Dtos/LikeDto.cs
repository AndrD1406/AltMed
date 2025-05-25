using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Dtos;

public class LikeDto
{
    public Guid Id { get; set; }

    public Guid PublicationId { get; set; }

    public string AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public bool IsLiked { get; set; }
}
