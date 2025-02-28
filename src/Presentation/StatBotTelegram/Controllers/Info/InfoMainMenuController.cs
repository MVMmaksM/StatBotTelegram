using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class InfoMainMenuController(ITelegramBotClient botClient, ICache cache)
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
                await cache.SetStateMenu(message.Chat.Id, MenuItems.GetInfoOrganization, cancellationToken);
                break;
            //Получить перечень форм
            case NameButton.GetListForms:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем сосотояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.GetListForm, cancellationToken);
                break;
            //Назад
            case NameButton.Back:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                //устанавливаем сосотояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.MainMenu, cancellationToken);
                break;
            default:
                textMessage = TextMessage.UnknownCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                //сбрасываем состояние команды
                await cache.RemoveOperationCode(message.Chat.Id, cancellationToken);
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