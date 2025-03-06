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
    public async Task<(string, List<InfoOrganization>)> GetInfoOrganization(RequestInfoForm requestInfo, CancellationToken ct)
    {
        var result = "По Вашему запросу организации не найдены!";
        
        List<InfoOrganization> infoOrg = null;
        //проверяем кэш
        infoOrg = await cacheRedis.GetInfoOrganization(requestInfo, ct);

        if (infoOrg == null)
        {
            var responce =
                await requesterApi.PostAsync<RequestInfoForm, List<InfoOrganization>, ErrorInfoOrganization>
                    ("/webstat/api/gs/organizations", requestInfo, ct);

            if (responce.Error != null)
            {
                return (responce.Error.ToDto(), new List<InfoOrganization>());
            }

            if (responce.Content != null)
            {
                infoOrg = responce.Content;
                //если есть записи, то пишем их в кэш
                if(infoOrg.Any())
                    await cacheRedis.SetInfoOrganization(infoOrg, requestInfo, ct); 
            }
        }
        
       if (infoOrg.Count == 1)
           result =  infoOrg.ToFullDto();

       if (infoOrg.Count > 1)
           result = $"По Вашему запросу найдено организаций: {infoOrg.Count}\n" +
                    $"Для того, чтобы получить данные по конкретной организации, выберите ОКПО " +
                    $"из списка ниже:\n\n" + infoOrg.ToShortDto();
        
        return (result, infoOrg);
    }
}