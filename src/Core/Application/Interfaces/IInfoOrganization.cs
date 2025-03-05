using Application.Models;

namespace Application.Interfaces;

public interface IInfoOrganization
{
    Task<string> GetInfoOrganization(RequestInfoForm requestInfo, CancellationToken cancellationToken);
}