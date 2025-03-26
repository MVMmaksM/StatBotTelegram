using Newtonsoft.Json;

namespace Application.Models.Templates;

public class ResponceTemplate
{
    [JsonProperty(PropertyName = "totalRows")]
    public int TotalRows { get; set; }
    
    [JsonProperty(PropertyName = "rows")]
    public List<Template> Rows { get; set; }
}