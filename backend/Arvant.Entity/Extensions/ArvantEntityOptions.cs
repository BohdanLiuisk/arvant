using Arvant.Entity.Descriptor;
using Microsoft.EntityFrameworkCore;

namespace Arvant.Entity.Extensions;

public class ArvantEntityOptions
{
    public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    
    public Func<Task<IEnumerable<TableDescriptor>>>? LoadTablesFromOuterStore { get; set; }
    
    public string? ConnectionString { get; set; }
}
