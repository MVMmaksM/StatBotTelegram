using Newtonsoft.Json;

namespace WorkerUpdateEmployees.Model;

public class Contact
{
    [JsonProperty(PropertyName = "okud")]
    public string Okud { get; set; }
    
    [JsonProperty(PropertyName = "form_index")]
    public string FormIndex { get; set; }
    
    [JsonProperty(PropertyName = "period")]
    public string Period { get; set; }
    
    [JsonProperty(PropertyName = "ekat_tel")]
    public string EkatTel { get; set; }
    
    [JsonProperty(PropertyName = "kurgan_tel")]
    public string KurganTel { get; set; }
}