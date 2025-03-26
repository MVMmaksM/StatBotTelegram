using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Application.Models;

public class RequestInfoForm
{
    [JsonProperty(PropertyName = "okpo")]
    public string? Okpo { get; set; } = string.Empty;
    
    [JsonProperty(PropertyName = "inn")]
    public string? Inn { get; set; } = string.Empty;
    
    [JsonProperty(PropertyName = "ogrn")]
    public string? Ogrn { get; set; } = string.Empty;
}