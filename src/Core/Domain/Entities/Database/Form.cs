namespace Domain.Entities;

public class Form : BaseEntity
{
    public string Name { get; set; } = null!;
    public int Okud { get; set; }
    public int PeriodicityFormId { get; set; }

    public PeriodicityForm PeriodicityForm { get; set; } = null!;
    public List<Employee> Employees { get; set; } = null!;
    
    public List<EmployeeForm> EmployeesForms { get; set; }
}