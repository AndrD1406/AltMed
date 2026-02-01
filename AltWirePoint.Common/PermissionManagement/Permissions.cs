using System.ComponentModel.DataAnnotations;

namespace AltWirePoint.Common.PermissionManagement;

public enum Permissions : short
{
    NotSet = 0, // error condition

    [Display(GroupName = "SystemManaging", Name = "SuperAdmin", Description = "access to all actions covered with [HasPermission] attribute")]
    AccessAll = short.MaxValue,

    #region Publication Management
    [Display(GroupName = "Publications", Name = "Read", Description = "Can view public content")]
    ReadPublications = 10,

    [Display(GroupName = "Publications", Name = "Create", Description = "Can post new media/articles")]
    CreatePublications = 11,

    [Display(GroupName = "Publications", Name = "EditOwn", Description = "Can edit their own posts")]
    EditOwnPublications = 12,

    [Display(GroupName = "Publications", Name = "DeleteOwn", Description = "Can delete their own posts")]
    DeleteOwnPublications = 13,

    [Display(GroupName = "Publications", Name = "Moderate", Description = "Can edit/delete ANY publication")]
    ModeratePublications = 14,
    #endregion

    #region Social Interactions
    [Display(GroupName = "Social", Name = "Like", Description = "Can like/react to posts")]
    LikePublication = 20,

    [Display(GroupName = "Social", Name = "Comment", Description = "Can comment on posts")]
    CommentOnPublication = 21,
    #endregion

    #region User Management
    [Display(GroupName = "Admin", Name = "ViewUsers", Description = "Can see user list")]
    ViewUsers = 50,

    [Display(GroupName = "Admin", Name = "BanUsers", Description = "Can ban or suspend users")]
    BanUsers = 51,
    #endregion
}
