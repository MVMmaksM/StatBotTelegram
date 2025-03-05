using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Services;

public class InfoOrganizationService(IRequesterApi requesterApi, ICache cacheRedis) : IInfoOrganization
{
    public async Task<string> GetInfoOrganization(RequestInfoForm requestInfo, CancellationToken ct)
    {
        string result = String.Empty;
        //проверяем кэш
        result = await cacheRedis.GetInfoOrganization(requestInfo, ct);

        if (result == null)
        {
            var responce = await requesterApi.PostAsync<RequestInfoForm>("/webstat/api/gs/organizations", requestInfo, ct);
        
            if (responce.IsSuccessStatusCode)
            {
                var dataResponce = await responce.Content.ReadAsStringAsync();
                var infoOrg = JsonConvert
                    .DeserializeObject<List<Models.InfoOrganization>>(dataResponce);
                
                result = infoOrg.ToDto();
                
                //если список непустой, то пишем в кэш  
                if(infoOrg.Any())
                    await cacheRedis.SetInfoOrganization(infoOrg[0], result, ct);
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
        }

        return result;
    }
}