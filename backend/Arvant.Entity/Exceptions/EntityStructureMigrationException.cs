using Arvant.Entity.ResultModels;

namespace Arvant.Entity.Exceptions;

public class EntityStructureMigrationException(MigrationResult migrationResult) 
    : EntityStructureException($"Couldn't migrate entity structure. Error: {migrationResult.Exception?.Message}");
