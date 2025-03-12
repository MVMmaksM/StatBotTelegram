using Application.Models;

namespace Application.Interfaces;

public interface IListForm
{
    Task<ResultRequest<List<Form>, string>> GetFormsById(string orgId, CancellationToken cancellationToken);
}