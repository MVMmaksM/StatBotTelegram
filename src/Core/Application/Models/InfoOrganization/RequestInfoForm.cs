using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Application.Models;

public class RequestInfoForm
{
    [JsonProperty(PropertyName = "okpo")]
    public string? Okpo { get; set; }
    
    [JsonProperty(PropertyName = "inn")]
    public string? Inn { get; set; }
    
    [JsonProperty(PropertyName = "ogrn")]
    public string? OgrnOgrnip { get; set; }
}