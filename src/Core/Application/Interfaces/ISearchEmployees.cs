using Application.Models.SearchEmployees;

namespace Application.Interfaces;

public interface ISearchEmployees
{
    Task<string> GetEmployees(RequestSearchEmployees request, CancellationToken cancellationToken);
}