using Newtonsoft.Json;

namespace Application.Models;

public class ErrorInfoOrganization
{
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }
}