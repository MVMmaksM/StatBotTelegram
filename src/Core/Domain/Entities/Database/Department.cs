namespace Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Employee> Employees { get; set; } = null!;
}