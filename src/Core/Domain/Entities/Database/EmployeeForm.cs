using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class EmployeeForm : BaseEntity
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("form_id")]
    public int FormId { get; set; }
    public Form Form { get; set; }
    
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    [Column("department_id")]
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}