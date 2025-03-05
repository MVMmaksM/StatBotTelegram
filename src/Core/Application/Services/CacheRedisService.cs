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
    public async Task<UserState> SetOperationCode(long chatId, OperationCode operationCode, CancellationToken cancellationToken)
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
    public async Task<string> GetInfoOrganization(RequestInfoForm requestInfo, CancellationToken cancellationToken)
    {
        var result = string.Empty;
        
        if (requestInfo.Okpo != String.Empty)
        {
            var key = string.Concat("infoOkpo_", requestInfo.Okpo);
            result = await cacheRedis.GetStringAsync(key, cancellationToken);
        }

        if (requestInfo.Ogrn != string.Empty)
        {
            var key = string.Concat("infoOgrn_", requestInfo.Ogrn);
            result = await cacheRedis.GetStringAsync(key, cancellationToken);
        }
        
        if (requestInfo.Inn != string.Empty)
        {
            var key = string.Concat("infoInn_", requestInfo.Inn);
            result = await cacheRedis.GetStringAsync(key, cancellationToken);
        }
        
        return result;
    }

    public async Task SetInfoOrganization(Models.InfoOrganization organization, string info, CancellationToken cancellationToken)
    {
        var keys = new List<string>()
        {
            string.Concat("infoOkpo_", organization.Okpo),
            string.Concat("infoOgrn_", organization.Ogrn),
            string.Concat("infoInn_", organization.Inn)
        };

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        foreach (var key in keys)
        {
            var infoStr = await cacheRedis.GetStringAsync(key, cancellationToken);

            if (infoStr == null)
            {
                await cacheRedis.SetStringAsync(key, info, cacheOptions, cancellationToken);
            }
        }
    }
}