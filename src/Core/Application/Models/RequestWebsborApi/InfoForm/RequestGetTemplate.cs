using Newtonsoft.Json;

namespace Application.Models;

public class RequestGetTemplate
{
    public RequestGetTemplate(string okud)
    {
        Filter = new FilterRequestGetTemplate(okud);
    }
    
    [JsonProperty(PropertyName = "filter")]
    public FilterRequestGetTemplate Filter { get; set; }
    
    [JsonProperty(PropertyName = "isSortAsc")]
    public bool IsSortAc { get; set; } = true;

    [JsonProperty(PropertyName = "pageIndex")]
    public int PageIndex { get; set; } = 0;
    
    [JsonProperty(PropertyName = "rowCount")]
    public int RowCount { get; set; } = 200;
}