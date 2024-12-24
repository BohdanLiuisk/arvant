using Arvant.Entity.ResultModels;
using Arvant.Entity.Structure;

namespace Arvant.Entity.Abstract;

public interface IEntityDbEngine
{
    MigrationResult MigrateNewEntityStructures(IReadOnlyList<EntityStructure> entityStructures);

    MigrationResult MigrateExistingEntityStructure(EntityStructure entityStructure);
}
