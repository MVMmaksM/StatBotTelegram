using Application.Constants;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StatBotTelegram.Controllers;

public class InfoOrganizationHandleController(ITelegramBotClient botClient, IStateUser stateUser, IInfoOrganizationService infoOrganizationService)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = stateUser.GetState(message.Chat.Id);
        if (state.OperationItem is not null && 
            (message.Text != "Назад" && message.Text != "По ОКПО" && message.Text != "По ИНН" && message.Text != "По ОГРН/ОГРНИП"))
        {
            await HandleOperation(message, cancellationToken);
        }
        else
        {
            await HandleButton(message, cancellationToken); 
        }
    }

    private async Task HandleButton(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "По ОКПО":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SearchOkpo}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchOkpo);
                break;
            case "По ИНН":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SearchInn}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchInn);
                break;
            case "По ОГРН/ОГРНИП":
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SearchOgrnOgrnip}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchOgrnOgrnip);
                break;
            case "Назад":
                //отправляем ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsInfoCodesAndListForm);
                //меняем состояние меню
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetInfoCodesAndListForm);
                //скидываем состояние выбранной операции
                stateUser.RemoveOperationCode(message.Chat.Id);
                break;
            default:
                //по умолчанию отправляем кнопки меню
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.UnknownCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                    //скидываем состояние выбранной операции
                    stateUser.RemoveOperationCode(message.Chat.Id);
                break;
        }
    }
    private async Task HandleOperation(Message message, CancellationToken cancellationToken)
    {
        var operationState = stateUser.GetState(message.Chat.Id).OperationItem;
        
        switch (operationState)
        {
            case OperationCode.SearchOkpo:
                var filterOrganization = new FilterOrganization()
                {
                    Okpo = message.Text,
                    Inn = string.Empty,
                    OgrnOgrnip = string.Empty
                };
                var res = await infoOrganizationService.GetInfoOrganization(filterOrganization, cancellationToken);
                //валидация введенного ОКПО
                //сервис получения данных организации
                //ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: res,
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
                break;
        }
    }
}