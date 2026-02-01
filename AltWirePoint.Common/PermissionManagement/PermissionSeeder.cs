namespace AltWirePoint.Common.PermissionManagement;

/// <summary>
/// basic seed for PermissionsForRole table in DB due to current state of application
/// </summary>
public class PermissionSeeder
{
    private static readonly IEnumerable<Permissions> UserPermissions = new List<Permissions>
    {
        Permissions.ReadPublications,
        Permissions.CreatePublications,
        Permissions.EditOwnPublications,
        Permissions.DeleteOwnPublications,
        Permissions.LikePublication,
        Permissions.CommentOnPublication
    };

    private static readonly IEnumerable<Permissions> AdminPermissions = new List<Permissions>
    {
        // Admins get everything
        Permissions.AccessAll,
        Permissions.ModeratePublications,
        Permissions.ViewUsers,
        Permissions.BanUsers
    };

    public static string? SeedPermissions(string role)
    {
        switch (role.ToLower())
        {
            case "user":
                return UserPermissions.PackPermissionsIntoString();
            case "admin":
                return AdminPermissions.PackPermissionsIntoString();
            default:
                break;
        }

        return null;
    }
}
