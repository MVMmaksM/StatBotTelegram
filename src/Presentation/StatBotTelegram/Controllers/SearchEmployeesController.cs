using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StatBotTelegram.Controllers;

public class SearchEmployeesController(ITelegramBotClient botClient, IStateUser stateUser)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = stateUser.GetState(message.Chat.Id);
        if (state.OperationItem is not null && message.Text != "Назад")
        {
            await HandleOperation(message, cancellationToken);
        }
        else
        {
            await HandleButton(message, cancellationToken); 
        }
    }

    /// <summary>
    /// обработка операций
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    private async Task HandleOperation(Message message, CancellationToken cancellationToken)
    {
        var operationState = stateUser.GetState(message.Chat.Id).OperationItem;
        
        switch (operationState)
        {
            case OperationCode.SearchOkud:
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"Введенный окуд неверный",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchEmployeesMenu);
                break;
        }
    }

    /// <summary>
    /// обработка нажати яна кнопки
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    private async Task HandleButton(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "По ОКУД формы":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SearchOkud}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchEmployeesMenu);
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchOkud);
                break;
            case "Назад":
                //отправляем ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsMainMenu);
                //меняем состояние меню
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.MainMenu);
                //скидываем состояние выбранной операции
                stateUser.RemoveOperationCode(message.Chat.Id);
                break;
            default:
                //по умолчанию отправляем кнопки меню
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.UnknownCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchEmployeesMenu);
                break;
        }
    }
}