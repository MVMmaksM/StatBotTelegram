namespace Domain.Entities;

public class Form : BaseEntity
{
    public string Name { get; set; }
    public int Okud { get; set; }
    
    public PeriodicityForm PeriodicityForm { get; set; }
    public List<Employee> Employees { get; set; }
}