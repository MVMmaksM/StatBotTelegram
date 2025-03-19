using Application.Models;

namespace Application.Interfaces;

public interface IExcelFileGen
{
    Task<byte[]> GetFileInfoOrg(List<InfoOrganization> infoOrg, CancellationToken ct);
    Task<byte[]> GetFileListForm(List<Form> forms, string okpo, CancellationToken ct);
}