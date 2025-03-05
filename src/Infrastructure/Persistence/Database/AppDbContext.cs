using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Database.ConfigureExtensions;

namespace Persistence.Database;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Form> Forms { get; set; }
    public DbSet<Employee> Employees { get; set; }
   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("StatBotTelegram"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureForms();
        modelBuilder.ConfigureDepartment();
        modelBuilder.ConfigureEmployee();
        modelBuilder.ConfigurePeriodicityForm();
        modelBuilder.ConfigureEmployeeForm();
    }
}