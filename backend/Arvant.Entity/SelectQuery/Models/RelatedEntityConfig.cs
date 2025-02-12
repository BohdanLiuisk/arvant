﻿using System.Text.Json.Serialization;

namespace Arvant.Entity.SelectQuery.Models;

public class RelatedEntityConfig
{
    [JsonPropertyName("name")]
    [JsonRequired]
    public string Name { get; set; } = null!;

    [JsonPropertyName("entityName")]
    [JsonRequired]
    public string EntityName { get; set; } = null!;

    [JsonPropertyName("joinBy")]
    [JsonRequired]
    public string JoinBy { get; set; } = null!;
    
    [JsonPropertyName("filters")]
    public List<SelectFilter> Filters { get; set; } = new();
    
    [JsonPropertyName("expressions")]
    public List<SelectExpression> Expressions { get; set; } = new();
    
    [JsonPropertyName("relatedEntities")]
    public List<RelatedEntityConfig> RelatedEntities { get; set; } = new();
}
