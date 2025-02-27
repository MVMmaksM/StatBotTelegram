using Application.Constants;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using FluentValidation;
using FluentValidation.Results;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class ListFormController(
    ITelegramBotClient botClient, 
    IStateUser stateUser, 
    IValidator<RequestInfoForm> validatorRequestInfoForm,
    IListForm listFormService)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = stateUser.GetState(message.Chat.Id);
        if (state.OperationItem is not null &&
            (message.Text != "Назад" && message.Text != "По ОКПО" && message.Text != "По ИНН" &&
             message.Text != "По ОГРН/ОГРНИП"))
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
            case "По ОКПО":
                textMessage = ConstTextMessage.SearchOkpo;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchOkpo);
                break;
            case "По ИНН":
                textMessage = ConstTextMessage.SearchInn;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchInn);
                break;
            case "По ОГРН/ОГРНИП":
                textMessage = ConstTextMessage.SearchOgrnOgrnip;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //устанавливаем состояние выбранной команды
                stateUser.SetOperationCode(message.Chat.Id, OperationCode.SearchOgrnOgrnip);
                break;
            case "Назад":
                textMessage = ConstTextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                //меняем состояние меню
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetInfoCodesAndListForm);
                //скидываем состояние выбранной операции
                stateUser.RemoveOperationCode(message.Chat.Id);
                break;
            default:
                //по умолчанию кнопки меню
                textMessage = ConstTextMessage.UnknownCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn;
                //скидываем состояние выбранной операции
                stateUser.RemoveOperationCode(message.Chat.Id);
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
        var operationState = stateUser.GetState(message.Chat.Id).OperationItem;
        var filter = new RequestInfoForm();
        ValidationResult validationResult = null;
        //в зависимости от выбранной операции 
        //составляем фильтр
        switch (operationState)
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
        
        var result = !validationResult.IsValid ? 
            validationResult.Errors.ToDto() : 
            await listFormService.GetListForm(filter, cancellationToken);
        
        //ответ
        await botClient.SendMessage(chatId: message.Chat.Id,
            protectContent: false, replyParameters: message.Id,
            text: result,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken,
            replyMarkup: KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
    }
}