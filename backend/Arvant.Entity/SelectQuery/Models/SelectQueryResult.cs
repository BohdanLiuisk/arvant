using System.Text.Json.Serialization;
namespace Arvant.Entity.SelectQuery.Models;

public class SelectQueryResult<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
    
    [JsonPropertyName("count")]
    public long Count { get; set; }
    
    [JsonPropertyName("paginationResult")]
    public PaginationResult? PaginationResult { get; set; }
    
    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = new();
}
