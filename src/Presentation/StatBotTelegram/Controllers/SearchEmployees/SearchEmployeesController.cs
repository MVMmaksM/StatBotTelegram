using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class SearchEmployeesController(ITelegramBotClient botClient, ICache cache)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = await cache.GetUserState(message.Chat.Id, cancellationToken);
        if (state.OperationItem is not null && 
            (message.Text != NameButton.BACK && message.Text != NameButton.BY_OKUD && message.Text != NameButton.BY_FIO && 
             message.Text != NameButton.BY_PHONE_EMPLOYEE))
        {
            await HandleOperation(message, cancellationToken);
        }
        else
        {
            await HandleButton(message, cancellationToken); 
        }
    }
    private async Task HandleOperation(Message message, CancellationToken cancellationToken)
    {
        var operationState = await cache.GetUserState(message.Chat.Id, cancellationToken);
        
        switch (operationState.OperationItem)
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
    private async Task HandleButton(Message message, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;
        
        switch (message.Text)
        {
            //По ОКУД формы
            case NameButton.BY_OKUD:
                textMessage = TextMessage.SearchOkud;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchOkud, cancellationToken);
                break;
            //По фамилии специалиста
            case NameButton.BY_FIO:
                textMessage = TextMessage.SearchFioEmployee;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchFio, cancellationToken);
                break;
            //По номеру телефона специалиста
            case NameButton.BY_PHONE_EMPLOYEE:
                textMessage = TextMessage.SearchPhoneEmployee;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchPhone, cancellationToken);
                break;
            //Назад
            case NameButton.BACK:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                //меняем состояние меню
                await cache.SetStateMenu(message.Chat.Id, MenuItems.MainMenu, cancellationToken);
                //скидываем состояние выбранной операции
                await cache.RemoveOperationCode(message.Chat.Id, cancellationToken);
                break;
            default:
                textMessage = TextMessage.UnknownCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                break;
        }
        //отправляем ответ
        await botClient.SendMessage(chatId: message.Chat.Id, 
            protectContent: true, replyParameters: message.Id,
            text: textMessage,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken,
            replyMarkup: buttonMenu);
    }
}