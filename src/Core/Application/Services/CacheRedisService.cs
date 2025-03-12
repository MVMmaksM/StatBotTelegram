using System.Collections.Concurrent;
using Application.Constants;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Services;

public class CacheRedisService(IDistributedCache cacheRedis) : ICache
{
    /// <summary>
    /// скидываем операцию пользователя
    /// </summary>
    /// <param name="chatId"></param>
    public async Task RemoveOperationCode(long chatId, CancellationToken cancellationToken)
    {
        var key = string.Concat("userState_", chatId);
        var userStateStr = await cacheRedis.GetStringAsync(key, cancellationToken);
        var userState = JsonConvert.DeserializeObject<UserState>(userStateStr);
        userState.OperationItem = null;
        await cacheRedis.SetStringAsync(key, JsonConvert.SerializeObject(userState), cancellationToken);
    }

    /// <summary>
    /// устанавливаем команду пользователю
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="operationCode"></param>
    /// <returns></returns>
    public async Task<UserState> SetOperationCode(long chatId, OperationCode operationCode,
        CancellationToken cancellationToken)
    {
        var key = string.Concat("userState_", chatId);
        var userStateStr = await cacheRedis.GetStringAsync(key, cancellationToken);
        var userState = JsonConvert.DeserializeObject<UserState>(userStateStr);
        userState.OperationItem = operationCode;
        await cacheRedis.SetStringAsync(key, JsonConvert.SerializeObject(userState), cancellationToken);

        return userState;
    }

    /// <summary>
    /// получаем состояние меню для пользователя
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    public async Task<UserState>? GetUserState(long chatId, CancellationToken cancellationToken)
    {
        var key = string.Concat("userState_", chatId);
        var userStateStr = await cacheRedis.GetStringAsync(key, cancellationToken);

        return userStateStr != null ? JsonConvert.DeserializeObject<UserState>(userStateStr) : null;
    }

    /// <summary>
    /// устанавливаем состояние меню для пользователя
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    public async Task<UserState> SetStateMenu(long chatId, MenuItems menuItem, CancellationToken cancellationToken)
    {
        UserState userState = null;
        var key = string.Concat("userState_", chatId);
        var userStateStr = await cacheRedis.GetStringAsync(key, cancellationToken);

        if (userStateStr != null)
        {
            userState = JsonConvert.DeserializeObject<UserState>(userStateStr);
            userState.MenuItem = menuItem;
            await cacheRedis.SetStringAsync(key, JsonConvert.SerializeObject(userState), cancellationToken);
            return userState;
        }
        else
        {
            //если для данного пользователя нет состояния,
            //то создаем
            userState = new UserState()
            {
                MenuItem = menuItem
            };

            await cacheRedis.SetStringAsync(key, JsonConvert.SerializeObject(userState), cancellationToken);
        }

        return userState;
    }

    public async Task<List<InfoOrganization>?> GetInfoOrganization(RequestInfoForm requestInfo,
        CancellationToken cancellationToken)
    {
        List<InfoOrganization>? organizations = null;
        var key = string.Empty;

        if (requestInfo.Okpo != string.Empty)
            key = string.Concat("infoOkpo_", requestInfo.Okpo);

        if (requestInfo.Inn != string.Empty)
            key = string.Concat("infoInn_", requestInfo.Inn);
        
        var dataCache = await cacheRedis.GetStringAsync(key, cancellationToken);
        if (dataCache != null)
        {
            organizations = JsonConvert.DeserializeObject<List<InfoOrganization>>(dataCache);
        }

        return organizations;
    }

    public async Task SetInfoOrganization(List<InfoOrganization> organizations, RequestInfoForm requestInfo,
        CancellationToken cancellationToken)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };
        var serialize = JsonConvert.SerializeObject(organizations);
        
        if (requestInfo.Okpo != string.Empty)
        {
            //сохраняем весь список с одним ключом
            var key = string.Concat("infoOkpo_", requestInfo.Okpo);
            await cacheRedis.SetStringAsync(key, serialize, cacheOptions, cancellationToken);

            //если в списке больше 1 организации
            //значит введен ОКПО головного подразделения
            if (organizations.Count() > 1)
            {
                //сохраняем каждую организацию со своим ключом
                foreach (var infoOrg in organizations.Where(o => o.Okpo != requestInfo.Okpo))
                {
                    var listInfoOrg = new List<InfoOrganization>();
                    listInfoOrg.Add(infoOrg);
                    await cacheRedis.SetStringAsync(
                        $"infoOkpo_{infoOrg.Okpo}",
                        JsonConvert.SerializeObject(listInfoOrg), cacheOptions, cancellationToken);
                }

                //сохраняем весь спико под одним ключом 
                //по ИНН, т.к. если в списке > 1 организации
                //то это головное подразделение
                //и у всех организаций одинаковый ИНН
                var inn = organizations[0].Inn;
                await cacheRedis.SetStringAsync($"infoInn_{inn}", serialize, cacheOptions, cancellationToken);
            }
        }

        if (requestInfo.Inn != string.Empty && organizations.Count() > 1)
        {
            var inn = organizations[0].Inn;
            await cacheRedis.SetStringAsync($"infoInn_{inn}", serialize, cacheOptions, cancellationToken);
        }
    }

    public async Task<List<Form>?> GetForms(string orgId, CancellationToken cancellationToken)
    {
        List<Form> forms = null;
        var cache = await cacheRedis.GetStringAsync($"listForms_{orgId}", cancellationToken);
        if(cache != null)
            forms = JsonConvert.DeserializeObject<List<Form>>(cache);
        
        return forms;
    }

    public async Task SetForms(string orgId, List<Form> forms, CancellationToken cancellationToken)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        
        await cacheRedis.SetStringAsync($"listForms_{orgId}", JsonConvert.SerializeObject(forms), cancellationToken);
    }
}