using Application.Models;

namespace Application.Interfaces;

public interface IInfoOrganizationService
{
    Task<string> GetInfoOrganization(FilterOrganization filter, CancellationToken cancellationToken);
}