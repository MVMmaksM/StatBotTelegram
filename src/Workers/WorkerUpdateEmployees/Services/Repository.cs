using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using WorkerUpdateEmployees.Data;
using WorkerUpdateEmployees.Interfaces;

namespace WorkerUpdateEmployees.Services;

public class Repository(IServiceScopeFactory scopeFactory) : IRepository
{
    public async Task<int> UpdateContactsAsync(string jsonContacts)
    {
        var parameter = new NpgsqlParameter("contact", NpgsqlDbType.Jsonb)
        {
            Value = jsonContacts
        };
           
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await dbContext.Database.ExecuteSqlRawAsync("CALL update_contact(@contact)", parameter);
    }
}