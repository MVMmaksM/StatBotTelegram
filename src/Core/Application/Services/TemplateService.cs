using Application.Interfaces;
using Application.Models;
using Application.Models.Templates;

namespace Application.Services;

public class TemplateService(IRequesterApi requesterApiService) : ITemplateService
{
    public async Task<ResultRequest<ResponceTemplate, string>> GetTemplate(RequestGetTemplate requestGetTemplate, CancellationToken cancellationToken)
    {
        var responce = await requesterApiService.PutAsync<RequestGetTemplate, ResponceTemplate, string>
            ("/webstat/api/forms", requestGetTemplate, cancellationToken);

        return responce != null && responce.Content != null
            ? responce
            : new ResultRequest<ResponceTemplate, string>();
    }

    public async Task<ResultRequest<string, string>> GetGuidByTemplateId(string templateId, CancellationToken cancellationToken)
    {
        var responce = await requesterApiService.GetAsync<string, string>
            ($"/webstat/api/templates/{templateId}/xml", cancellationToken);

        return responce != null && responce.Content != null
            ? responce
            : new ResultRequest<string, string>();
    }

    public async Task<ResultRequest<string, string>> DownloadTemplateByGiud(string templateGuid, CancellationToken cancellationToken)
    {
        var responce = await requesterApiService.Download
            ($"/webstat/api/files/tmp/{templateGuid}", cancellationToken);

        return responce != null && responce.Content != null
            ? responce
            : new ResultRequest<string, string>();
    }
}