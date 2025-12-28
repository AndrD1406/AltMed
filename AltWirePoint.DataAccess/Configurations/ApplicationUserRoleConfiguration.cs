using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.DataAccess.Configurations;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasData(
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"), // admin user
                RoleId = Guid.Parse("3f1d2e4c-5b6a-7d8e-9f01-2a3b4c5d6e7f")  // Admin role
            },
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("9c8b8e2e-4f3a-4d2e-bf4a-e5c8a1b2c3d4"), // regular user
                RoleId = Guid.Parse("4a5b6c7d-8e9f-0a1b-2c3d-4e5f6a7b8c9d")  // User role
            }
        );
    }
}
