using Microsoft.EntityFrameworkCore;

namespace WorkerUpdateEmployees.Data;

public class AppDbContext(IConfiguration configuration, IHostEnvironment environment) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("pg_db"))
            .EnableSensitiveDataLogging(environment.IsDevelopment());
    }
}