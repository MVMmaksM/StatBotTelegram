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
        RequestInfoForm request,
        CancellationToken ct)
    {
        //проверяем кэш
        List<InfoOrganization> cache = await cacheRedis.GetInfoOrganization(request, ct);

        if (cache == null)
        {
            var responce =
                await requesterApi.PostAsync<RequestInfoForm, List<InfoOrganization>, ErrorInfoOrganization>
                    ("/webstat/api/gs/organizations", request, ct);

            if (responce.Content != null && responce.Content.Any())
                await cacheRedis.SetInfoOrganization(responce.Content, request, ct);

            return responce;
        }

        return new ResultRequest<List<InfoOrganization>, ErrorInfoOrganization>()
        {
            Content = cache,
            Error = null
        };
    }
}