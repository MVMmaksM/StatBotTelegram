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
    public async Task<ResultRequest<List<InfoOrganization>, ErrorInfoOrganization>> GetInfoOrganization(
        RequestInfoForm requestInfo,
        CancellationToken ct)
    {
        //проверяем кэш
        var cache = await cacheRedis.GetInfoOrganization(requestInfo, ct);

        if (cache == null)
        {
            var responce =
                await requesterApi.PostAsync<RequestInfoForm, List<InfoOrganization>, ErrorInfoOrganization>
                    ("/webstat/api/gs/organizations", requestInfo, ct);

            if (responce.Content.Any())
                await cacheRedis.SetInfoOrganization(responce.Content, requestInfo, ct);

            return responce;
        }

        return new ResultRequest<List<InfoOrganization>, ErrorInfoOrganization>()
        {
            Content = cache,
            Error = null
        };
    }
}