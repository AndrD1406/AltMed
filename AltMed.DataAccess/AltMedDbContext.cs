using AltMed.DataAccess.Identity;
using AltMed.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.DataAccess;

public class AltMedDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public virtual DbSet<Publication> Publications { get; set; }
    
    public virtual DbSet<Like> Likes { get; set; }

    public AltMedDbContext(DbContextOptions<AltMedDbContext> options)
        : base(options)
    {
    }

    public AltMedDbContext()
    { 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=AltMedDb;Username=postgres;Password=1234"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AltMedDbContext).Assembly);
    }
}
