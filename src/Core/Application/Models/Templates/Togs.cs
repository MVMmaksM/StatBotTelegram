using Newtonsoft.Json;

namespace Application.Models.Templates;

public class Togs
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
}