namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? SurName { get; set; }
    public long Phone { get; set; }
    public int DepartmentId { get; set; }

    public Department? Department { get; set; } 
    public List<Form> Forms { get; set; } = null!;
}