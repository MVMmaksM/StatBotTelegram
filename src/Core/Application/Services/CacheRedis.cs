using System.Collections.Concurrent;
using Application.Constants;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Services;

public class CacheRedis(IDistributedCache cacheRedis) : ICache
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
    public async Task<UserState>? GetState(long chatId, CancellationToken cancellationToken)
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

    public bool ExistStateMenu(long chatId)
    {
        return true;
    }
}