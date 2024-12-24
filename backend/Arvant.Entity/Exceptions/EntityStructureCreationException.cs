using Arvant.Entity.ResultModels;

namespace Arvant.Entity.Exceptions;

public class EntityStructureCreationException : EntityStructureException
{
    public EntityStructureCreationException(IList<CreateEntityStructureResult> createEntityResults)
        : base("Couldn't build entity structures. See errors in json.", SerializeResults(createEntityResults)) { }
    
    public EntityStructureCreationException(CreateEntityStructureResult createEntityStructureResult)
        : base("Couldn't build entity structure. See errors in json.", SerializeResult(createEntityStructureResult)) { }
}
