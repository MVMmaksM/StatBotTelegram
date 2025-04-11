using Microsoft.EntityFrameworkCore;

namespace WorkerUpdateEmployees.Data;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("pg_db"))
            .EnableSensitiveDataLogging();
    }
}