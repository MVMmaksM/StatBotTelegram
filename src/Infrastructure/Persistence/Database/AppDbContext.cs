using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persisitence.Database.ConfigureExtensions;

namespace Persisitence.Database;

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
    }
}