using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using StatBotTelegram.Extensions;
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
            case NameButton.GET_INFO_ORGANIZATION:
                textMessage = TextMessage.SELECT_COMMAND;
                buttonMenu = KeyboradButtonMenu.ButtonsGetInfoOrg;
                //устанавливаем состояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.GetInfoOrganization, cancellationToken);
                break;
            //Получить перечень форм
            case NameButton.GET_LIST_FORMS:
                var downloadTemplateButton = new KeyboardButton(NameButton.DOWNLOAD_TEMPLATE);
                textMessage = TextMessage.SELECT_COMMAND;
                buttonMenu = KeyboradButtonMenu.ButtonsGetListForm;
                //устанавливаем сосотояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.GetListForm, cancellationToken);
                break;
            //Назад
            case NameButton.BACK:
                textMessage = TextMessage.SELECT_COMMAND;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                //устанавливаем сосотояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.MainMenu, cancellationToken);
                break;
            default:
                textMessage = TextMessage.UNKNOWN_COMMAND;
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