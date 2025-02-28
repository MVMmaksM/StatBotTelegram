using Application.Constants;
using Application.Models;

namespace Application.Interfaces;

public interface ICache
{
    Task<UserState>? GetState(long chatId, CancellationToken cancellationToken);
    Task<UserState> SetStateMenu(long chatId, MenuItems menuItem, CancellationToken cancellationToken);
    Task<UserState>  SetOperationCode(long chatId, OperationCode operationCode, CancellationToken cancellationToken);
    Task RemoveOperationCode(long chatId, CancellationToken cancellationToken);
    bool ExistStateMenu(long chatId);
}