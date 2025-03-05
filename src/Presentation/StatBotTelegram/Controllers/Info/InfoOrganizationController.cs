using Application.Constants;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using FluentValidation;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentValidation.Results;
using StatBotTelegram.Helpers;

namespace StatBotTelegram.Controllers;

public class InfoOrganizationController(
    ITelegramBotClient botClient,
    ICache cache,
    IInfoOrganization infoOrganization,
    IValidator<RequestInfoForm> validatorRequestInfoForm)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = await cache.GetUserState(message.Chat.Id, cancellationToken);
        if (state.OperationItem is not null &&
            (message.Text != NameButton.BACK && message.Text != NameButton.BY_OKPO && message.Text != NameButton.BY_INN &&
             message.Text != NameButton.BY_OGRN))
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
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;
        
        //в зависимости от выбранной кнопки
        //устанавливаем кнопки, сообщение и состояние
        switch (message.Text)
        {
            //По ОКПО
            case NameButton.BY_OKPO:
                textMessage = TextMessage.SEARCH_OKPO;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchOkpo, cancellationToken);
                break;
            //По ИНН
            case NameButton.BY_INN:
                textMessage = TextMessage.SEACRH_INN;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchInn, cancellationToken);
                break;
            //По ОГРН/ОГРНИП
            case NameButton.BY_OGRN:
                textMessage = TextMessage.SEARXH_OGRN;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchOgrnOgrnip, cancellationToken);
                break;
            //Назад
            case NameButton.BACK:
                textMessage = TextMessage.SELECT_COMMAND;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                //меняем состояние меню
                await cache.SetStateMenu(message.Chat.Id, MenuItems.InfoMainMenu, cancellationToken);
                //скидываем состояние выбранной операции
                await cache.RemoveOperationCode(message.Chat.Id, cancellationToken);
                break;
            default:
                //по умолчанию кнопки меню
                textMessage = TextMessage.UNKNOWN_COMMAND;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //скидываем состояние выбранной операции
                await cache.RemoveOperationCode(message.Chat.Id, cancellationToken);
                break;
        }
        
        //отправляем ответ
        await botClient.SendMessage(chatId: message.Chat.Id,
            protectContent: true, replyParameters: message.Id,
            text: textMessage,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken,
            replyMarkup: buttonMenu);
    }

    private async Task HandleOperation(Message message, CancellationToken cancellationToken)
    {
        var operationState = await cache.GetUserState(message.Chat.Id, cancellationToken);
        var filter = new RequestInfoForm();
        ValidationResult validationResult = null;
        //в зависимости от выбранной операции 
        //составляем фильтр
        switch (operationState.OperationItem)
        {
            case OperationCode.SearchOkpo:
                //составляем фильтр
                filter.Okpo = message.Text.Trim();
                //валидация
                validationResult = await validatorRequestInfoForm.ValidateAsync(filter);
                break;
            case OperationCode.SearchInn:
                //составляем фильтр
                filter.Inn = message.Text.Trim();
                //валидация
                validationResult = await validatorRequestInfoForm.ValidateAsync(filter);
                break;
            case OperationCode.SearchOgrnOgrnip:
                //составляем фильтр
                filter.Ogrn = message.Text.Trim();
                //валидация
                validationResult = await validatorRequestInfoForm.ValidateAsync(filter);
                break;
        }
        
        var result = !validationResult.IsValid ? 
            validationResult.Errors.ToDto() : 
            await infoOrganization.GetInfoOrganization(filter, cancellationToken);
        
        var splitMessages = SplitterMessage.SplitMessage(result);

        foreach (var messagePart in splitMessages)
        {
            //ответ
            await botClient.SendMessage(chatId: message.Chat.Id,
                protectContent: false, replyParameters: message.Id,
                text: messagePart ,
                parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
        }
    }
}