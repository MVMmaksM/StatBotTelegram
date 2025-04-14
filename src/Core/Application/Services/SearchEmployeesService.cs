using Application.Extensions;
using Application.Interfaces;
using Application.Models.SearchEmployees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;

namespace Application.Services;

public class SearchEmployeesService(IServiceScopeFactory scopeFactory) : ISearchEmployees
{
    public async Task<string> GetEmployees(RequestSearchEmployees request, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var query = dbContext
            .Forms
            .Include(f => f.PeriodicityForm)
            .AsQueryable();

        if (request.Okud != string.Empty)
        {
            int.TryParse(request.Okud, out int okudInt);
            query = query.Where(f => f.Okud == okudInt);
        }

        /*if (request.FioEmployee != string.Empty)
            query = query.Where(f=> f.Employees.Where(e => e.LastName.Contains(request.FioEmployee)).Any());

        if(request.IndexForm != string.Empty)
            query= query.Where(f=> f.Name.Contains(request.IndexForm));

        if (request.PhoneEmployee != string.Empty)
        {
            query = query.Where(f => f.Employees.Where(e => e.Phone.Contains(request.PhoneEmployee)).Any());
        }*/

        var forms = await query.ToListAsync(cancellationToken);

        return forms.ToDto();
    }
}