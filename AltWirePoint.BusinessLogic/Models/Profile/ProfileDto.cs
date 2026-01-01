namespace AltWirePoint.BusinessLogic.Models.Profile;

public class ProfileDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public string Logo { get; set; } = default!;
    public List<Guid> PublicationIds { get; set; } = new();
}