namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? SurName { get; set; }
    public string Phone { get; set; } = null!;
    public int DepartmentId { get; set; }

    public Department Department { get; set; } 
    public List<Form> Forms { get; set; } = null!;

    public List<EmployeeForm> EmployeesForms { get; set; }
}