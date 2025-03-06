using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using Newtonsoft.Json;

namespace Application.Services;

public class ListFormService(IRequesterApi requesterApi) : IListForm
{
    public async Task<string> GetListForm(RequestInfoForm requestInfo, CancellationToken cancellationToken)
    {
        string result = String.Empty;
        List<InfoOrganization> infoOrg = null;
        var responce = await requesterApi.PostAsync<RequestInfoForm, List<InfoOrganization>, string>
            ("/webstat/api/gs/organizations", requestInfo, cancellationToken);
        
        if(responce.Error != null)
            return responce.Error;

        if (responce.Content != null)
            infoOrg = responce.Content;
        
        if (!infoOrg.Any())
        {
            result = "По Вашему запросу организации не найдены!";
        }
        else if(infoOrg.Count == 1)
        {
            result = await GetForms(infoOrg[0].Id, cancellationToken);
        }
        else if (infoOrg.Count > 1)
        {
            result = $"По Вашему запросу найдено организаций: {infoOrg.Count}\n" +
                     $"Для того, чтобы получить переченб форм по конкретной организации, выберите критерий поиска" +
                     $" \"По ОКПО\" и введите один ОКПО из списка ниже:\n\n";
            result += infoOrg.ToShortDto();
        }
        
        return result;
    }

    private async Task<string> GetForms(string organizationId, CancellationToken cancellationToken)
    {
        var result = string.Empty;
        var responce = await requesterApi.GetAsync<List<Form>, string>
            ($"/webstat/api/gs//organizations/{organizationId}/forms", cancellationToken);

        if(responce.Error != null)
            return responce.Error;
        
        if(responce.Content != null)
            return responce.Content.ToDto();

        return "Формы не найдены!";
    }
}