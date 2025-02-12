﻿using System.Text.Json.Serialization;

namespace Arvant.Entity.Structure;

public class EntityIndexStructure
{
    [JsonConstructor]
    internal EntityIndexStructure(string entityName, string name, bool isUnique, IEnumerable<string> columns) {
        EntityName = entityName;
        Name = name;
        IsUnique = isUnique;
        Columns = columns;
    }

    [JsonIgnore] 
    public EntityStructure EntityStructure { get; internal set; } = null!;
    
    [JsonPropertyName("entityName")]
    public string EntityName { get; private set; }
    
    [JsonPropertyName("name")]
    public string Name { get; private set; }
    
    [JsonPropertyName("isUnique")]
    public bool IsUnique { get; private set; }
    
    [JsonPropertyName("columns")]
    public IEnumerable<string> Columns { get; private set; }
    
    [JsonIgnore]
    public EntityStructureElementState State { get; internal set; }

    internal static EntityIndexStructure CreateUniqueIndex(EntityStructure entityStructure, string columnName) {
        var indexName = GetUniqueIndexName(entityStructure.Name, columnName);
        var columns = new[] { columnName };
        var index = new EntityIndexStructure(entityStructure.Name, indexName, isUnique: true, columns) {
            EntityStructure = entityStructure,
            State = EntityStructureElementState.New
        };
        return index;
    }
    
    private static string GetUniqueIndexName(string tableName, string columnName) {
        return $"ix_{tableName}_{columnName}_unique";
    }
}
