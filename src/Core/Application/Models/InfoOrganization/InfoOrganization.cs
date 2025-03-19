using Newtonsoft.Json;

namespace Application.Models;

public class InfoOrganization
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "okpo")]
    public string Okpo { get; set; }
    
    [JsonProperty(PropertyName = "date_reg")]
    public string DateReg { get; set; }
    
    [JsonProperty(PropertyName = "inn")]
    public string Inn { get; set; }
    
    [JsonProperty(PropertyName = "ogrn")]
    public string Ogrn { get; set; }
    
    [JsonProperty(PropertyName = "okato_fact")]
    public OkatoFact OkatoFact{ get; set; }
    
    [JsonProperty(PropertyName = "okato_reg")]
    public OkatoReg OkatoReg{ get; set; }
    
    [JsonProperty(PropertyName = "oktmo_fact")]
    public OktmoFact OktmoFact{ get; set; }
    
    [JsonProperty(PropertyName = "oktmo_reg")]
    public OktmoReg OktmoReg{ get; set; }
    
    [JsonProperty(PropertyName = "okogu")]
    public Okogu Okogu{ get; set; }
    
    [JsonProperty(PropertyName = "okfs")]
    public Okfs Okfs{ get; set; }
    
    [JsonProperty(PropertyName = "okopf")]
    public Okfs Okopf{ get; set; }
}