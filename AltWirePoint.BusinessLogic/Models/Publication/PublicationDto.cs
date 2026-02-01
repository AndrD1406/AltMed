namespace AltWirePoint.BusinessLogic.Models.Publication;

public class PublicationDto
{
    public Guid Id { get; set; }
    public string? Content { get; set; }

    public string? Image64 { get; set; }

    public DateTime PostedAt { get; set; }

    public Guid AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorLogo { get; set; }

    public List<LikeDto>? Likes { get; set; }

    public List<CommentDto>? Comments { get; set; }
}
