using AltWirePoint.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AltWirePoint.DataAccess.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        var admin = new ApplicationUser
        {
            Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            UserName = "admin@AltWirePoint.local",
            NormalizedUserName = "ADMIN@AltWirePoint.LOCAL",
            Email = "admin@AltWirePoint.local",
            NormalizedEmail = "ADMIN@AltWirePoint.LOCAL",
            EmailConfirmed = true,

            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),

            RefreshToken = null,
            RefreshTokenExpirationDateTime = DateTime.MinValue
        };
        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

        var user = new ApplicationUser
        {
            Id = Guid.Parse("9c8b8e2e-4f3a-4d2e-bf4a-e5c8a1b2c3d4"),
            UserName = "user@AltWirePoint.local",
            NormalizedUserName = "USER",
            Email = "USER@AltWirePoint.LOCAL",
            NormalizedEmail = "USER@AltWirePoint.LOCAL",
            EmailConfirmed = true,

            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),

            RefreshToken = null,
            RefreshTokenExpirationDateTime = DateTime.MinValue
        };
        user.PasswordHash = hasher.HashPassword(user, "User123!");

        builder.HasData(admin, user);
    }
}