using Application.Models;
using Application.Models.Templates;

namespace Application.Interfaces;

public interface ITemplateService
{
    Task<ResultRequest<ResponceTemplate, string>> GetTemplate(RequestGetTemplate requestGetTemplate, CancellationToken cancellationToken);
    Task<ResultRequest<string, string>> GetGuidByTemplateId(string templateId, CancellationToken cancellationToken);
    Task<ResultRequest<string, string>> DownloadTemplateByGiud(string templateGuid, CancellationToken cancellationToken);
}