using System.Collections.Concurrent;
using Application.Constants;
using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class StorageStateUser : IStateUser
{
    private readonly ConcurrentDictionary<long, StateUser> _stateUser;

    public StorageStateUser()
    {
        _stateUser = new ConcurrentDictionary<long, StateUser>();
    }

    /// <summary>
    /// скидываем операцию пользователя
    /// </summary>
    /// <param name="chatId"></param>
    public void RemoveOperationCode(long chatId)
    {
        _stateUser[chatId].OperationItem = null;
    }

    /// <summary>
    /// устанавливаем команду пользователю
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="operationCode"></param>
    /// <returns></returns>
    public StateUser SetOperationCode(long chatId, OperationCode operationCode)
    {
        _stateUser[chatId].OperationItem = operationCode;
        return _stateUser[chatId];
    }

    /// <summary>
    /// получаем состояние меню для пользователя
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    public StateUser? GetState(long chatId)
        =>_stateUser.ContainsKey(chatId) ? _stateUser[chatId] : null;

    /// <summary>
    /// устанавливаем состояние меню для пользователя
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    public StateUser SetStateMenu(long chatId, MenuItems menuItem)
    {
        //если есть, то просто меняем меню
        if (_stateUser.ContainsKey(chatId))
        {
            _stateUser[chatId].MenuItem = menuItem;
            return _stateUser[chatId];
        }
        
        //если для данного пользователя нет состояния,
        //то создаем
        var stateUserMenu = new StateUser()
        {
            MenuItem = menuItem
        };

        _stateUser.TryAdd(chatId, stateUserMenu);
        return _stateUser[chatId];
    }

    public bool ExistStateMenu(long chatId)
    {
        throw new NotImplementedException();
    }
}