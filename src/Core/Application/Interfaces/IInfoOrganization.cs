using Application.Models;

namespace Application.Interfaces;

public interface IInfoOrganization
{
    Task<(string, List<InfoOrganization>)> GetInfoOrganization(RequestInfoForm requestInfo, CancellationToken cancellationToken);
}