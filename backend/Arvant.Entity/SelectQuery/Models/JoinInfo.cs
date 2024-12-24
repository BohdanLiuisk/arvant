using Arvant.Entity.Structure;

namespace Arvant.Entity.SelectQuery.Models;

public class JoinInfo(string joinPath, string alias, EntityStructure primaryStructure, 
    EntityStructure parentStructure)
{
    public string JoinPath { get; } = joinPath;

    public string Alias { get; } = alias;

    public EntityStructure PrimaryStructure { get; } = primaryStructure;

    public EntityStructure ParentStructure { get; } = parentStructure;
}
