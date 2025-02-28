using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class InfoMainMenuController(ITelegramBotClient botClient, IStateUser stateUser)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;
        
        switch (message.Text)
        {
            //Получить данные о кодах статистики организации
            case NameButton.GetInfoOrganization:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем сосотояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetInfoOrganization);
                break;
            //Получить перечень форм
            case NameButton.GetListForms:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем сосотояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetListForm);
                break;
            //Назад
            case NameButton.Back:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                //устанавливаем сосотояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.MainMenu);
                break;
            default:
                textMessage = TextMessage.UnknownCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                //сбрасываем состояние команды
                stateUser.RemoveOperationCode(message.Chat.Id);
                break;
        }
        
        //ответ
        await botClient.SendMessage(chatId: message.Chat.Id, 
            protectContent: true, replyParameters: message.Id,
            text: textMessage,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken,
            replyMarkup: buttonMenu);
    }
}