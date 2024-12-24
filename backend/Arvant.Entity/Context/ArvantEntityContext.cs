using Arvant.Entity.Structure;
using Microsoft.EntityFrameworkCore;

namespace Arvant.Entity.Context;

public class ArvantEntityContext(DbContextOptions<ArvantEntityContext> options) : DbContext(options)
{
    public DbSet<EntityStructure> EntityStructures { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new EntityStructureTypeConfig());
    }
}
