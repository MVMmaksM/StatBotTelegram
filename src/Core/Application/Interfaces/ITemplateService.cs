using Application.Models;
using Application.Models.Templates;

namespace Application.Interfaces;

public interface ITemplateService
{
    Task<ResultRequest<ResponceTemplate, string>> GetTemplate(RequestGetTemplate requestGetTemplate, CancellationToken cancellationToken);
}