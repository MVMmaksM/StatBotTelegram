using Application.Models;

namespace Application.Interfaces;

public interface IInfoOrganizationService
{
    Task<string> GetInfoOrganization(RequestInfoOrganization requestInfo, CancellationToken cancellationToken);
}