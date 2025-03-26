using Newtonsoft.Json;

namespace Application.Models;

public class FilterRequestGetTemplate
{
    public FilterRequestGetTemplate(string okud)
    {
        //если ОКУД введен как 6 знаков и без ведущего 0
        //то добавляем ведущий 0
        Okud = okud.Length == 6 && !okud.StartsWith("0") ? string.Concat("0", okud) : okud;
    }
    
    [JsonProperty(PropertyName = "okud")]
    public string Okud { get; set; }
    
    [JsonProperty(PropertyName = "isActual")]
    public bool IsActual { get; set; } = true;
    
    [JsonProperty(PropertyName = "isArchive")]
    public bool IsArchive { get; set; } = false;
}