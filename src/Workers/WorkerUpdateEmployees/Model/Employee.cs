using Newtonsoft.Json;

namespace WorkerUpdateEmployees.Model;

public class Employee
{
    [JsonProperty(PropertyName = "firstname")]
    public string FirstName { get; set; }
    
    [JsonProperty(PropertyName = "lastname")]
    public string LastName { get; set; }
    
    [JsonProperty(PropertyName = "surname")]
    public string SurName { get; set; }
    
    [JsonProperty(PropertyName = "phone")]
    public string Phone { get; set; }
    
    [JsonProperty(PropertyName = "department")]
    public string Department { get; set; }
}