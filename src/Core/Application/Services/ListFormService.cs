using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using Newtonsoft.Json;

namespace Application.Services;

public class ListFormService(IRequesterApi requesterApiService, ICache cacheRedis) : IListForm
{
    public async Task<ResultRequest<List<Form>, string>> GetFormsById(string orgId, CancellationToken cancellationToken)
    {
        //проверяем кэш
        List<Form> cache = await cacheRedis.GetForms(orgId, cancellationToken);

        if (cache == null)
        {
            var responce =  await requesterApiService.GetAsync<List<Form>, string>
                ($"/webstat/api/gs//organizations/{orgId}/forms", cancellationToken);
            
            if (responce.Content != null && responce.Content.Any())
                await cacheRedis.SetForms(orgId, responce.Content, cancellationToken);

            return responce;
        }
        
        return new ResultRequest<List<Form>, string>()
        {
            Content = cache,
            Error = null
        };
    }
}