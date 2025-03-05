namespace Domain.Entities;

public class PeriodicityForm : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Form>? Forms { get; set; } 
}