namespace AltWirePoint.BusinessLogic.Models;

public class LikeDto
{
    public Guid Id { get; set; }

    public Guid PublicationId { get; set; }

    public string AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public bool IsLiked { get; set; }
}
