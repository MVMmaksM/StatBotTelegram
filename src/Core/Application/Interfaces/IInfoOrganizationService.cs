using Application.Models;

namespace Application.Interfaces;

public interface IInfoOrganizationService
{
    Task<string> GetInfoOrganization(RequestInfoForm requestInfo, CancellationToken cancellationToken);
}