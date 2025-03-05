using Application.Constants;
using Application.Extensions;
using Application.Interfaces;
using Application.Models.SearchEmployees;
using FluentValidation;
using FluentValidation.Results;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class SearchEmployeesController
    (
        ITelegramBotClient botClient, 
        ICache cache,
        IValidator<RequestSearchEmployees> validatorRequestSearchEmployees,
        ISearchEmployees searchEmployeesService)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var state = await cache.GetUserState(message.Chat.Id, cancellationToken);
        if (state.OperationItem is not null && 
            (message.Text != NameButton.BACK && message.Text != NameButton.BY_OKUD && message.Text != NameButton.BY_FIO && 
             message.Text != NameButton.BY_PHONE_EMPLOYEE && message.Text != NameButton.BY_INDEX_FORM))
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
        var operationState = await cache.GetUserState(message.Chat.Id, cancellationToken);
        
        var textMessage = string.Empty;
        RequestSearchEmployees request = null;
        ValidationResult validationResult = null;
        
        switch (operationState.OperationItem)
        {
            //по ОКУД
            case OperationCode.SearchOkud:
                request = new RequestSearchEmployees
                {
                    Okud = message.Text.Trim()
                };
                break;
            //По ФИО
            case OperationCode.SearchFio:
                request = new RequestSearchEmployees
                {
                    FioEmployee = message.Text.Trim()
                };
                break;
            //По номеру телефона специалиста
            case OperationCode.SearchPhoneEmployee:
                request = new RequestSearchEmployees
                {
                    PhoneEmployee = message.Text.Trim()
                };
                break;
            //По индексу формы
            case OperationCode.SearchIndexForm:
                request = new RequestSearchEmployees
                {
                    IndexForm = message.Text.Trim()
                };
                break;
        }
        
        validationResult = await validatorRequestSearchEmployees.ValidateAsync(request, cancellationToken);
        var result = !validationResult.IsValid ? 
            validationResult.Errors.ToDto() : 
            await searchEmployeesService.GetEmployees(request, cancellationToken);
        
        //ответ
        await botClient.SendMessage(chatId: message.Chat.Id, 
            protectContent: true, replyParameters: message.Id,
            text: result,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken,
            replyMarkup: KeyboradButtonMenu.ButtonsSearchEmployeesMenu);
    }
    private async Task HandleButton(Message message, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;
        
        switch (message.Text)
        {
            //По ОКУД формы
            case NameButton.BY_OKUD:
                textMessage = TextMessage.SEARCH_OKUD;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchOkud, cancellationToken);
                break;
            //По фамилии специалиста
            case NameButton.BY_FIO:
                textMessage = TextMessage.SEARCH_FIO_EMPLOYEE;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchFio, cancellationToken);
                break;
            //По номеру телефона специалиста
            case NameButton.BY_PHONE_EMPLOYEE:
                textMessage = TextMessage.SEARCH_PHONE_EMPLOYEE;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchPhoneEmployee, cancellationToken);
                break;
            //По индексу формы
            case NameButton.BY_INDEX_FORM:
                textMessage = TextMessage.SEACRH_INDEX_FORM;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                //устанавливаем состояние выбранной команды
                await cache.SetOperationCode(message.Chat.Id, OperationCode.SearchIndexForm, cancellationToken);
                break;
            //Назад
            case NameButton.BACK:
                textMessage = TextMessage.SELECT_COMMAND;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                //меняем состояние меню
                await cache.SetStateMenu(message.Chat.Id, MenuItems.MainMenu, cancellationToken);
                //скидываем состояние выбранной операции
                await cache.RemoveOperationCode(message.Chat.Id, cancellationToken);
                break;
            default:
                textMessage = TextMessage.UNKNOWN_COMMAND;
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