using DemonstrateProjects.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemonstrateProjects.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<PersonalReadKey> PersonalReadKeys => Set<PersonalReadKey>();
}