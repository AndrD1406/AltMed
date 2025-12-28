using AltWirePoint.DataAccess.Identity;
using AltWirePoint.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.DataAccess;

public class AltWirePointDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public virtual DbSet<Publication> Publications { get; set; }
    
    public virtual DbSet<Like> Likes { get; set; }

    public AltWirePointDbContext(DbContextOptions<AltWirePointDbContext> options)
        : base(options)
    {
    }

    public AltWirePointDbContext()
    { 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=AltWirePointDb;Username=postgres;Password=1234"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AltWirePointDbContext).Assembly);
    }
}
