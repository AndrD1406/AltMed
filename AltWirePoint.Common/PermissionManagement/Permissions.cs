using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AltWirePoint.Common.PermissionManagement;

public enum Permissions : short
{
    [Display(GroupName = "SystemManaging", Name = "SuperAdmin", Description = "access to all actions covered with [HasPermission] attribute")]
    AccessAll = short.MaxValue,
}
