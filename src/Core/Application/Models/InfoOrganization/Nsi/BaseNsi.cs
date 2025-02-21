using Newtonsoft.Json;

namespace Application.Models;

public abstract class BaseNsi
{
    [JsonProperty(PropertyName = "id")]
    public double Id { get; set; }
    
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }
    
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "parent_id")]
    public string? ParentId { get; set; }
}