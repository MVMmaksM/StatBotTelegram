using Application.Models;

namespace Application.Interfaces;

public interface IInfoOrganization
{
    Task<ResultRequest<List<InfoOrganization>, ErrorInfoOrganization>> GetInfoOrganization(RequestInfoForm request, CancellationToken cancellationToken);
}