using System.ComponentModel.DataAnnotations;

namespace AltWirePoint.BusinessLogic.Models.Identity;

public class ChangePasswordRequest
{
    [Required] public string CurrentPassword { get; set; } = default!;
    [Required] public string NewPassword { get; set; } = default!;
    [Required] public string ConfirmPassword { get; set; } = default!;
}
