using Arvant.Entity.Structure;

namespace Arvant.Entity.ResultModels;

public class ChangedStructureElement
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required EntityStructureAction Action { get; set; }
}
