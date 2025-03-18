using Application.Models;

namespace Application.Interfaces;

public interface IExcelFileGen
{
    Task<byte[]> GetFileInfoOrg(List<InfoOrganization> infoOrg);
}