using Arvant.Application.Common.Interfaces;
using Arvant.Domain.Constants;
using Arvant.Domain.Entities;
using Arvant.Infrastructure.Context.Configurations;
using Arvant.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Arvant.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options), IArvantContext 
{
    public DbSet<User> InternalUsers { get; set; }
    
    public DbSet<Call> Calls { get; set; }
    
    public DbSet<CallParticipant> CallParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CallEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CallParticipantEntityTypeConfiguration());
        var roles = new List<IdentityRole> {
            new() { Name = RoleNames.Admin, NormalizedName = RoleNames.Admin.ToUpper() },
            new() { Name = RoleNames.Member, NormalizedName = RoleNames.Member.ToUpper() },
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}
