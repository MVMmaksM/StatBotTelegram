using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Persistence.Database;
using WorkerUpdateEmployees.Interfaces;

namespace WorkerUpdateEmployees.Services;

public class Repository(AppDbContext dbContext) : IRepository
{
    public async Task UpdateContactsAsync(string jsonContacts)
    {
        var parameter = new NpgsqlParameter("contact", NpgsqlDbType.Jsonb)
        {
            Value = jsonContacts
        };
           
        var res = await dbContext.Database.ExecuteSqlRawAsync("CALL update_contact(@contact)", parameter);
    }
}