using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class EmployeeForm : BaseEntity
{
    [Column("form_id")]
    public int FormId { get; set; }
    public Form Form { get; set; }
    
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}