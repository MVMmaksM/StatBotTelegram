using Newtonsoft.Json;

namespace Application.Models;

public class Form
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "okud")]
    public string Okud { get; set; }

    [JsonProperty(PropertyName = "form_period")]
    public string FormPeriod { get; set; }
    
    [JsonProperty(PropertyName = "formatted_period")]
    public string FormattedPeriod { get; set; }
    
    [JsonProperty(PropertyName = "reported_period")]
    public string ReportedPeriod { get; set; }
    
    [JsonProperty(PropertyName = "period")]
    public string Period { get; set; }
    
    [JsonProperty(PropertyName = "period_comment")]
    public string PeriodComment { get; set; }
    
    [JsonProperty(PropertyName = "dept_nsi_id")]
    public string DeptNsiId { get; set; }
    
    [JsonProperty(PropertyName = "dept_nsi_code")]
    public string DeptNsiCode { get; set; }
    
    [JsonProperty(PropertyName = "type_exam")]
    public string TypeExam { get; set; }
    
    [JsonProperty(PropertyName = "index")]
    public string Index { get; set; }
    
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }
    
    [JsonProperty(PropertyName = "act_num")]
    public string ActNum { get; set; }
    
    [JsonProperty(PropertyName = "act_date")]
    public string ActDate { get; set; }
    
    [JsonProperty(PropertyName = "end_time")]
    public string EndTime { get; set; }
    
    [JsonProperty(PropertyName = "comment")]
    public string Comment { get; set; }
    
    [JsonProperty(PropertyName = "updatingDate")]
    public string UpdatingDate { get; set; }
    
    [JsonProperty(PropertyName = "isValid")]
    public bool IsValid { get; set; }
    
    [JsonProperty(PropertyName = "periodicity")]
    public int Periodicity { get; set; }
    
    [JsonProperty(PropertyName = "periodNum")]
    public int PeriodNum { get; set; }
    
    [JsonProperty(PropertyName = "periodYear")]
    public int PeriodYear { get; set; }
}