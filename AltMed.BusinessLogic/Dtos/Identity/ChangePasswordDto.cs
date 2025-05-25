using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Dtos.Identity;

public class ChangePasswordDto
{
    [Required] public string CurrentPassword { get; set; } = default!;
    [Required] public string NewPassword { get; set; } = default!;
    [Required] public string ConfirmPassword { get; set; } = default!;
}
