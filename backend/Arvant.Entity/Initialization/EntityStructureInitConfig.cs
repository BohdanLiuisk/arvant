using Arvant.Entity.Descriptor;

namespace Arvant.Entity.Initialization;

public class EntityStructureInitConfig
{
    public IList<TableDescriptor>? OuterTables { get; init; }
}
