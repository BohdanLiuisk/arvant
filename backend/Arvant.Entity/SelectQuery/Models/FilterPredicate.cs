using System.Text.Json;
using System.Text.Json.Serialization;
using Arvant.Entity.SelectQuery.Enums;

namespace Arvant.Entity.SelectQuery.Models;

public class FilterPredicate
{
    [JsonPropertyName("operator")]
    [JsonRequired]
    public string Operator { get; set; } = null!;
    
    [JsonPropertyName("valueType")]
    public PredicateValueType ValueType { get; set; } = PredicateValueType.Value;
    
    [JsonPropertyName("value")]
    public JsonElement? Value { get; set; }
    
    [JsonPropertyName("datePart")]
    public string? DatePart { get; set; }
    
    [JsonPropertyName("caseSensitive")]
    public bool? CaseSensitive { get; set; }
    
    [JsonPropertyName("subQuery")]
    public SelectExpression? SubQuery { get; set; }
}
