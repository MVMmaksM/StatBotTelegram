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
        var responce = await requesterApi.PostAsync<RequestInfoForm>("/webstat/api/gs/organizations", requestInfo, cancellationToken);
        
        if (responce.IsSuccessStatusCode)
        {
            var dataResponce = await responce.Content.ReadAsStringAsync();
            var organizations = JsonConvert
                .DeserializeObject<List<Models.InfoOrganization>>(dataResponce);

            if (organizations.Count != 0)
            {
                var organizationId = organizations[0].Id;
                result = await GetForms(organizationId, cancellationToken);
            }
        }
        else if (responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var dataResponce = await responce.Content.ReadAsStringAsync();
            result = JsonConvert
                .DeserializeObject<ErrorInfoOrganization>(dataResponce)
                .ToDto();
        }
        else
        {
            result = "Сервис получения данных временно недоступен!";
        }
        
        return result;
    }

    private async Task<string> GetForms(string organizationId, CancellationToken cancellationToken)
    {
        var result = string.Empty;
        var responce = await requesterApi.GetAsync($"/webstat/api/gs//organizations/{organizationId}/forms", cancellationToken);

        if (responce.IsSuccessStatusCode)
        {
            var dataResponce = await responce.Content.ReadAsStringAsync();
            result = JsonConvert
                .DeserializeObject<List<Form>>(dataResponce)
                .ToDto();
        }
        else
        {
            result = "Сервис получения данных временно недоступен!";
        }
        
        return result;
    }
}