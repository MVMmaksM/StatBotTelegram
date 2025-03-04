namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SurName { get; set; }
    public long Phone { get; set; }

    public Department Department { get; set; }
    public List<Form> Forms { get; set; }
}