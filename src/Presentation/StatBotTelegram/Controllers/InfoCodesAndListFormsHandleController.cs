using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StatBotTelegram.Controllers;

public class InfoCodesAndListFormsHandleController(ITelegramBotClient botClient, IStateUser stateUser)
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

    private async Task HandleOperation(Message message, CancellationToken cancellationToken)
    {
        
    }

    /// <summary>
    /// обработка нажатия на кнопки
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    private async Task HandleButton(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "Получить данные о кодах статистики организации":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsGetInfoOrganization);
                break;
            case "Получить перечень форм":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsGetInfoOrganization);
                break;
            case "Назад":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsInfoCodesAndListForm);
                break;
        }
    }
}