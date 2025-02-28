using Application.Constants;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using FluentValidation;
using FluentValidation.Results;
using StatBotTelegram.Components;
using StatBotTelegram.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class ListFormController(
    ITelegramBotClient botClient,
    ICache cache,
    IValidator<RequestInfoForm> validatorRequestInfoForm,
    IListForm listFormService)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = await cache.GetUserState(message.Chat.Id, cancellationToken);
        if (state.OperationItem is not null &&
            (message.Text != NameButton.Back && message.Text != NameButton.ByOkpo && message.Text != NameButton.ByInn &&
             message.Text != NameButton.ByOgrn))
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
            case NameButton.ByOkpo:
                textMessage = TextMessage.SearchOkpo;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchOkpo, cancellationToken);
                break;
            //По ИНН
            case NameButton.ByInn:
                textMessage = TextMessage.SearchInn;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchInn, cancellationToken);
                break;
            //По ОГРН/ОГРНИП
            case NameButton.ByOgrn:
                textMessage = TextMessage.SearchOgrnOgrnip;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchOgrnOgrnip, cancellationToken);
                break;
            //Назад
            case NameButton.Back:
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                //меняем состояние меню
                await cache.SetStateMenu(message.Chat.Id, MenuItems.InfoMainMenu, cancellationToken);
                //скидываем состояние выбранной операции
                await cache.RemoveOperationCode(message.Chat.Id, cancellationToken);
                break;
            default:
                //по умолчанию кнопки меню
                textMessage = TextMessage.UnknownCommand;
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
                filter.Inn = string.Empty;
                filter.Ogrn = string.Empty;
                //валидация
                validationResult = await validatorRequestInfoForm.ValidateAsync(filter);
                break;
            case OperationCode.SearchInn:
                //составляем фильтр
                filter.Okpo = string.Empty;
                filter.Inn = message.Text.Trim();
                filter.Ogrn = string.Empty;
                //валидация
                validationResult = await validatorRequestInfoForm.ValidateAsync(filter);
                break;
            case OperationCode.SearchOgrnOgrnip:
                //составляем фильтр
                filter.Okpo = string.Empty;
                filter.Inn = string.Empty;
                filter.Ogrn = message.Text.Trim();
                //валидация
                validationResult = await validatorRequestInfoForm.ValidateAsync(filter);
                break;
        }

        var result = !validationResult.IsValid
            ? validationResult.Errors.ToDto()
            : await listFormService.GetListForm(filter, cancellationToken);

        var splitMessages = SplitterMessage.SplitMessage(result);

        foreach (var messagePart in splitMessages)
        {
            //ответ
            await botClient.SendMessage(chatId: message.Chat.Id,
                protectContent: false, replyParameters: message.Id,
                text: messagePart,
                parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
        }
    }
}