using Newtonsoft.Json;

namespace WorkerUpdateEmployees.Model;

public class Contact
{
    [JsonProperty(PropertyName = "okud")]
    public int Okud { get; set; }
    
    [JsonProperty(PropertyName = "form_index")]
    public string FormIndex { get; set; }
    
    [JsonProperty(PropertyName = "period")]
    public string Period { get; set; }
    
    [JsonProperty(PropertyName = "employees")]
    public Employee[] Employees { get; set; }
}