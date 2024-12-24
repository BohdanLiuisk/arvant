using Arvant.Entity.Structure;

namespace Arvant.Entity.ResultModels;

public class CreateEntityStructureResult() : EntityStructureActionResult(EntityStructureAction.Create)
{
    public CreateEntityStructureResult(EntityStructure entityStructure) : this() {
        ResultStructure = entityStructure;
        EntityName = entityStructure.Name;
    }
    
    public CreateEntityStructureResult(Exception? exception) : this() {
        if (exception != null) {
            SetException(exception);
        }
    }
}
