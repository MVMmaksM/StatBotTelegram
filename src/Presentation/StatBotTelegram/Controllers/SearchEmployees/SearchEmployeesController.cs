using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class SearchEmployeesController(ITelegramBotClient botClient, IStateUser stateUser)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = stateUser.GetState(message.Chat.Id);
        if (state.OperationItem is not null && 
            (message.Text != NameButton.Back && message.Text != NameButton.ByOkud && message.Text != NameButton.ByFio && 
             message.Text != NameButton.ByPhoneEmployee))
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
    private async Task HandleButton(Message message, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;
        
        switch (message.Text)
        {
            //По ОКУД формы
            case NameButton.ByOkud:
                textMessage = TextMessage.SearchOkud;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchOkud);
                break;
            //По фамилии специалиста
            case NameButton.ByFio:
                textMessage = TextMessage.SearchFioEmployee;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchFio);
                break;
            //По номеру телефона специалиста
            case NameButton.ByPhoneEmployee:
                textMessage = TextMessage.SearchPhoneEmployee;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchPhone);
                break;
            //Назад
            case NameButton.Back:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                //меняем состояние меню
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.MainMenu);
                //скидываем состояние выбранной операции
                stateUser.RemoveOperationCode(message.Chat.Id);
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