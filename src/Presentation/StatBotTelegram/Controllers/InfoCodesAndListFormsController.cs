using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StatBotTelegram.Controllers;

public class InfoCodesAndListFormsController(ITelegramBotClient botClient, IStateUser stateUser)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "Получить данные о кодах статистики организации":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                //устанавливаем сосотояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetInfoOrganization);
                break;
            case "Получить перечень форм":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                //устанавливаем сосотояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetListForm);
                break;
            case "Назад":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsMainMenu);
                //устанавливаем сосотояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.MainMenu);
                break;
            default:
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.UnknownCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsInfoCodesAndListForm);
                //сбрасываем состояние команды
                stateUser.RemoveOperationCode(message.Chat.Id);
                break;
        }
    }
}