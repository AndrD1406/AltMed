using AltWirePoint.DataAccess.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.DataAccess.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = Guid.Parse("3f1d2e4c-5b6a-7d8e-9f01-2a3b4c5d6e7f"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new ApplicationRole
            {
                Id = Guid.Parse("4a5b6c7d-8e9f-0a1b-2c3d-4e5f6a7b8c9d"),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );
    }
}
