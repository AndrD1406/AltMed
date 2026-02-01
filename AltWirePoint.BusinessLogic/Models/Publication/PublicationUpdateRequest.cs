namespace AltWirePoint.BusinessLogic.Models.Publication;

public class PublicationUpdateRequest
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? Image64 { get; set; }
}
