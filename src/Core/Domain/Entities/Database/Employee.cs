namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? SurName { get; set; }
    public string Phone { get; set; } = null!;

    public List<EmployeeForm> EmployeesForms { get; set; }
}