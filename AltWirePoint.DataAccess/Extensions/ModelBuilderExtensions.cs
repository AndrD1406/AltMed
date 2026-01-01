using AltWirePoint.DataAccess.Enums;
using AltWirePoint.DataAccess.Identity;
using AltWirePoint.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AltWirePoint.DataAccess.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder builder)
    {
        builder.Entity<PermissionsForRole>().HasData(
            new PermissionsForRole
            {
                Id = 1,
                RoleName = Role.User.ToString(),
                PackedPermissions = Common.PermissionManagement.PermissionSeeder.SeedPermissions(Role.User.ToString())!,
                Description = "Default permissions for user role"
            },
            new PermissionsForRole
            {
                Id = 2,
                RoleName = Role.Admin.ToString(),
                PackedPermissions = Common.PermissionManagement.PermissionSeeder.SeedPermissions(Role.Admin.ToString())!,
                Description = "Default permissions for admin role"
            }
        );

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                UserName = "admin@AltWirePoint.local",
                NormalizedUserName = "ADMIN@ALTWIREPOINT.LOCAL",
                Email = "admin@AltWirePoint.local",
                NormalizedEmail = "ADMIN@ALTWIREPOINT.LOCAL",
                EmailConfirmed = true,
                Role = "Admin",
                SecurityStamp = "78641975-6548-4394-814B-E08F4C873394",
                ConcurrencyStamp = "D0596395-5E13-4B9F-8B57-550797A2E29B",
                RefreshToken = null,
                RefreshTokenExpirationDateTime = DateTime.MinValue,  
                PasswordHash = "AQAAAAIAAYagAAAAEKFwG0rjyVGeyL3VFTvtdjIoHOnY3dtlBm9RGWi5AuWBII8HWVFYipPu4e0qC3t2uQ=="
            },
            new ApplicationUser
            {
                Id = Guid.Parse("9c8b8e2e-4f3a-4d2e-bf4a-e5c8a1b2c3d4"),
                UserName = "user@AltWirePoint.local",
                NormalizedUserName = "USER@ALTWIREPOINT.LOCAL",
                Email = "user@AltWirePoint.local",
                NormalizedEmail = "USER@ALTWIREPOINT.LOCAL",
                EmailConfirmed = true,
                Role = "User",
                SecurityStamp = "071C8A63-8F4C-4732-A2C3-54F5E87D0343",
                ConcurrencyStamp = "81907DB4-6C91-494E-9B2D-0F4F242A684C",
                RefreshToken = null,
                RefreshTokenExpirationDateTime = DateTime.MinValue,
                PasswordHash = "AQAAAAIAAYagAAAAEC2l7yHDJzpL5dE6/zWXxKASLKXXv2wb4tnJIdr7YT0rtuvgC1djEUeS1RjLEG3p3Q=="
            }
        );
    }
}
