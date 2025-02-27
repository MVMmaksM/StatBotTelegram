using Application.Models;

namespace Application.Interfaces;

public interface IListForm
{
    Task<string> GetListForm(RequestInfoForm requestInfo, CancellationToken cancellationToken);
}