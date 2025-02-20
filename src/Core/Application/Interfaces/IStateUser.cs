using Application.Constants;
using Application.Models;

namespace Application.Interfaces;

public interface IStateUser
{
    StateUser? GetState(long chatId);
    StateUser SetStateMenu(long chatId, MenuItems menuItem);
    StateUser SetOperationCode(long chatId, OperationCode operationCode);
    
    void RemoveOperationCode(long chatId);
    bool ExistStateMenu(long chatId);
}