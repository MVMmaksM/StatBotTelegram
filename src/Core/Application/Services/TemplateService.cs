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
}