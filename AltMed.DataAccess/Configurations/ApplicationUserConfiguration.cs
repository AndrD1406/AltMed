using AltMed.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AltMed.DataAccess.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        var admin = new ApplicationUser
        {
            Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            UserName = "admin@altmed.local",
            NormalizedUserName = "ADMIN@ALTMED.LOCAL",
            Email = "admin@altmed.local",
            NormalizedEmail = "ADMIN@ALTMED.LOCAL",
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
            UserName = "user@altmed.local",
            NormalizedUserName = "USER",
            Email = "USER@ALTMED.LOCAL",
            NormalizedEmail = "USER@ALTMED.LOCAL",
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