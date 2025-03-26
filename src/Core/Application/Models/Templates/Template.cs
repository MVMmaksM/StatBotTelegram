using Newtonsoft.Json;

namespace Application.Models.Templates;

public class Template
{
    
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }
    
    [JsonProperty(PropertyName = "version")]
    public string Version { get; set; }
    
    [JsonProperty(PropertyName = "togs")]
    public string Togs { get; set; }
}