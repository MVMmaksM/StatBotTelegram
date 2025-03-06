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

    public async Task<List<InfoOrganization>> GetInfoOrganization(RequestInfoForm requestInfo,
        CancellationToken cancellationToken)
    {
        List<InfoOrganization> organizations = null;

        if (requestInfo.Okpo != string.Empty)
        {
            var key = string.Concat("infoOkpo_", requestInfo.Okpo);
            var infoOkpoStr = await cacheRedis.GetStringAsync(key, cancellationToken);
            if (infoOkpoStr != null)
            {
                var infoOkpo = JsonConvert.DeserializeObject<InfoOrganization>(infoOkpoStr);
                organizations = new List<InfoOrganization>();
                organizations.Add(infoOkpo);
            }
        }

        if (requestInfo.Ogrn != string.Empty)
        {
            var key = string.Concat("infoOgrn_", requestInfo.Ogrn);
            var infoOgrnStr = await cacheRedis.GetStringAsync(key, cancellationToken);
            if(infoOgrnStr != null)
                organizations = new List<InfoOrganization>(JsonConvert.DeserializeObject<List<InfoOrganization>>(infoOgrnStr));
        }

        if (requestInfo.Inn != string.Empty)
        {
            var key = string.Concat("infoInn_", requestInfo.Inn);
            var infoInnStr = await cacheRedis.GetStringAsync(key, cancellationToken);
            if(infoInnStr != null)
                organizations = new List<InfoOrganization>(JsonConvert.DeserializeObject<List<InfoOrganization>>(infoInnStr));
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

        //если приходит запрос по ИНН или ОГРН
        //то сохраняем по ИНН и ОГРН список
        //и отдельно для каждого ОКПО
        if (requestInfo.Ogrn != string.Empty || requestInfo.Inn != string.Empty)
        {
            var keyInn = $"infoInn_{organizations[0].Inn}";
            var keyOgrn = $"infoOgrn_{organizations[0].Ogrn}";

            var organizationsStr = JsonConvert.SerializeObject(organizations);
            await cacheRedis.SetStringAsync(keyInn, organizationsStr, cacheOptions, cancellationToken);
            await cacheRedis.SetStringAsync(keyOgrn, organizationsStr, cacheOptions, cancellationToken);

            foreach (var org in organizations)
            {
                var keyOkpo = $"infoOkpo_{org.Okpo}";
                var orgStr = JsonConvert.SerializeObject(org);
                await cacheRedis.SetStringAsync(keyOkpo, orgStr, cacheOptions, cancellationToken);
            }
        }

        //если приходит ОКПО
        //т осохраняем только по ОКПО
        if (requestInfo.Okpo != string.Empty)
        {
            foreach (var org in organizations)
            {
                var keyOkpo = $"infoOkpo_{org.Okpo}";
                var orgStr = JsonConvert.SerializeObject(org);
                await cacheRedis.SetStringAsync(keyOkpo, orgStr, cacheOptions, cancellationToken);
            }
        }
    }
}