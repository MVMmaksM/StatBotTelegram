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
            (message.Text != NameButton.BACK && message.Text != NameButton.BY_OKPO &&
             message.Text != NameButton.BY_INN &&
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
        var textMessage = string.Empty;
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

        //убираем всю логику в отдельный метод
        var (splitMessages, inlineButtons) = await GenerateMessage(validationResult, filter, cancellationToken);
        
        for (int i = 0; i < splitMessages.Count(); i++)
        {
            //ответ
            await botClient.SendMessage(chatId: message.Chat.Id,
                protectContent: false, replyParameters: message.Id,
                text: splitMessages[i],
                parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                //если есть инлайнкнопки и сообщение последнее
                //то показываем эти кнопки
                //иначе показываем дефолтные менюшные кнопки
                replyMarkup: inlineButtons is not null && i == splitMessages.Count() - 1
                    ? inlineButtons
                    : KeyboradButtonMenu.ButtonsSearchOkpoInnOgrn);
        }
    }

    private async Task<(List<string>, InlineKeyboardButton[][])> GenerateMessage
        (ValidationResult validationResult, RequestInfoForm filter, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        InlineKeyboardButton[][] inlineButtons = null;

        try
        {
            //если есть ошибки валидации
            //то пишем в результат
            if (!validationResult.IsValid)
            {
                textMessage = validationResult.Errors.ToDto();
            }
            //если ошибок валидации не было
            else
            {
                //делаем запрос в сервис
                var resultRequest = await infoOrganization.GetInfoOrganization(filter, cancellationToken);
                //если не пришла ошибка
                if (resultRequest.Error == null)
                {
                    var infoOrg = resultRequest.Content;

                    //если пусто йсписко организаций пришел из сервиса
                    //то пишем пользователю, что организаций не найдено
                    if (infoOrg == null || infoOrg.Count() == 0)
                        textMessage = "По Вашему запросу организации не найдены!";

                    //если пришло больше одной организации
                    //то пишем, что найдено много организаций
                    if (infoOrg.Count() > 1)
                    {
                        textMessage = $"По Вашему запросу найдено организаций: {infoOrg.Count}\n" +
                                      $"Для того, чтобы получить данные по конкретной организации, выберите ОКПО " +
                                      $"из списка ниже:\n\n" + infoOrg.ToShortDto();

                        //и формируем кнопки для каждой организации
                        inlineButtons = CreateInlineKeyboardButtonInfoOrg
                            .Create<InfoOrganization>(objects:infoOrg, 
                                nameCallbackData: CallbackData.GET_INFO_ORG, 
                                propertyForCallbackData: "Okpo", 
                                propertyForTextButton: "Okpo", 
                                textForButton: "");
                    }

                    //если всего одна организация найдена
                    //то выдаем полную инфу по ней
                    if (infoOrg.Count() == 1)
                    {
                        textMessage = infoOrg.ToFullDto();
                        //и формируем кнопку получения списка форм для организации
                        inlineButtons = CreateInlineKeyboardButtonInfoOrg
                            .Create<InfoOrganization>(objects:infoOrg, 
                                nameCallbackData: CallbackData.GET_LIST_FORM, 
                                propertyForCallbackData: "Id", 
                                propertyForTextButton: null, 
                                textForButton: "Получить список форм");
                    }
                }
                //если пришла ошибка
                else
                {
                    //пишем ее в результат
                    textMessage = resultRequest.Error.ToDto();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            textMessage = "Произошла внутренняя ошибка!";
        }

        //делим сообщение на части, чтобы не превысить размер сообщения
        var splitMessages = SplitterMessage.SplitMessage(textMessage);

        return (splitMessages, inlineButtons);
    }
}