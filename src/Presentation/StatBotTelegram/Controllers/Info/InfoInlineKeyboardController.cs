using Application.Constants;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StatBotTelegram.Extensions;
using StatBotTelegram.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class InfoInlineKeyboardController(
    ITelegramBotClient botClient,
    IInfoOrganization infoOrgService,
    IListForm listFormService,
    IAbstractFactoryGenFile factoryGenFile)
{
    public async Task Handle(Update update, CancellationToken ct)
    {
        InlineKeyboardButton[][] buttons = null;
        var fileName = string.Empty;
        var textMessage = string.Empty;
        byte[] bytesFile = null;

        try
        {
            if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_INFO_ORG))
                (textMessage, buttons) = await GetInfoOrg(update, ct);

            if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_LIST_FORM))
                (textMessage, buttons) = await GetListForm(update, ct);
            //экспорт инфо об организации
            if (update.CallbackQuery.Data.StartsWith(CallbackData.EXPORT_EXCEL_INFO_ORG))
                (textMessage, fileName, bytesFile) = await ExportExcelInfoOrg(update, ct);
            //экспорт инфо перечня форм
            if (update.CallbackQuery.Data.StartsWith(CallbackData.EXPORT_EXCEL_LIST_FORM))
                (textMessage, fileName, bytesFile) = await ExportExcelListForm(update, ct);
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message} \n{e.StackTrace}");
            textMessage = TextMessage.INTERNAL_ERROR;
        }

        //если сообщение пустое, а файл нет
        //значит отправляем файл
        if (string.IsNullOrWhiteSpace(textMessage) && bytesFile != null)
        {
            await using var ms = new MemoryStream(bytesFile);
            await botClient.SendDocument(
                chatId: update.CallbackQuery.From.Id,
                document: InputFile.FromStream(ms, fileName));
        }
        else
        {
            //делим сообщение на части
            var splitMessages = SplitterMessage.SplitMessage(textMessage);
            //отправляем частями
            for (int i = 0; i < splitMessages.Count(); i++)
            {
                await botClient.SendMessage(
                    chatId: update.CallbackQuery.From.Id,
                    protectContent: false,
                    text: textMessage,
                    replyMarkup: i == splitMessages.Count() - 1 ? buttons : null,
                    parseMode: ParseMode.Html,
                    cancellationToken: ct);
            }
        }
    }

    private async Task<(string, string, byte[]?)> ExportExcelListForm(Update update, CancellationToken ct)
    {
        var fileName = string.Empty;
        var textMessage = string.Empty;
        byte[] bytesFile = null;
        ResultRequest<List<Form>, string> responce = null;

        //получаем генератор файла
        var excelFileGen = factoryGenFile.GetExcelFileGen();

        //получаем из callbackQuery ид и окпо организации
        var orgId = update.CallbackQuery.Data.Split("_")[1];
        var orgOkpo = update.CallbackQuery.Data.Split("_")[2];

        //дергаем сервис
        responce = await listFormService.GetFormsById(orgId, ct);

        //если есть ошибка, то пишем
        //в сообщение
        if (responce.Error != null)
            textMessage = responce.Error;

        //если есть данные
        if (responce.Content != null && responce.Content.Any())
        {
            var forms = responce
                .Content
                .ToList();

            //генерируем файл
            bytesFile = await excelFileGen.GetFileListForm(forms, orgOkpo, ct);
            fileName = "Перечень форм.xlsx";
        }
        else
        {
            textMessage = TextMessage.NOT_FOUND_LIST_FORM;
        }

        return (textMessage, fileName, bytesFile);
    }

    private async Task<(string, string, byte[]?)> ExportExcelInfoOrg(Update update, CancellationToken ct)
    {
        var fileName = string.Empty;
        var textMessage = string.Empty;
        byte[] bytesFile = null;
        ResultRequest<List<InfoOrganization>, ErrorInfoOrganization> responce = null;

        //получаем генератор файла
        var excelFileGen = factoryGenFile.GetExcelFileGen();

        //формируем запрос
        var request = new RequestInfoForm()
        {
            Okpo = update.CallbackQuery.Data.Split("_")[1]
        };

        //дергаем сервис
        responce = await infoOrgService.GetInfoOrganization(request, ct);

        //если есть ошибка
        //то пишем в сообщение
        if (responce.Error != null)
            textMessage = responce.Error.ToDto();

        //если есть данные
        if (responce.Content != null && responce.Content.Any())
        {
            //то выбираем из списка только одну запись
            //с введенным ОКПО
            var infoOrg = responce
                .Content
                .Where(o => o.Okpo == request.Okpo)
                .ToList();

            //генерируем файл
            bytesFile = await excelFileGen.GetFileInfoOrg(infoOrg, ct);
            fileName = "Информация об организации.xlsx";
        }
        else
        {
            textMessage = TextMessage.NOT_FOUND_INFO_ORG;
        }

        return (textMessage, fileName, bytesFile);
    }

    private async Task<(string, InlineKeyboardButton[][])> GetInfoOrg(Update update, CancellationToken ct)
    {
        var textMessage = string.Empty;
        ResultRequest<List<InfoOrganization>, ErrorInfoOrganization> responce = null;
        InlineKeyboardButton[][] buttons = null;

        //готовим запрос
        var request = new RequestInfoForm()
        {
            //получаем ОКПО из CallbackQuery
            Okpo = update.CallbackQuery.Data.Split("_")[1]
        };

        //дергаем сервис
        responce = await infoOrgService.GetInfoOrganization(request, ct);

        //если ошибка
        //пишем в сообщение
        if (responce.Error != null)
            textMessage = responce.Error.ToDto();

        //если есть данные
        if (responce.Content.Any())
        {
            //выбираем из списка одну организацию
            //по выбранному ОКПО
            var infoOrg = responce
                .Content
                .Where(o => o.Okpo == request.Okpo)
                .ToList();
            
            //готовим кнопку получения перечня форм
            var buttonGetForms = 
                new InlineKeyboardButton(text: NameButton.GET_LIST_FORMS,
                    callbackDataOrUrl: $"{CallbackData.GET_LIST_FORM}_{infoOrg.First().Id}_{infoOrg.First().Okpo}");
            //готовим кнопку экспорта
            var buttonExport =
                new InlineKeyboardButton(text: NameButton.EXPORT,
                    callbackDataOrUrl: $"{CallbackData.EXPORT_EXCEL_INFO_ORG}_{responce.Content.First().Okpo}");

            //добавляем кнопки
            buttons = new InlineKeyboardButton[0][]
                .AddButton(buttonGetForms)
                .AddButton(buttonExport);

            textMessage = infoOrg.ToFullDto();
        }
        else
        {
            textMessage = TextMessage.NOT_FOUND_INFO_ORG;
        }


        return (textMessage, buttons);
    }

    private async Task<(string, InlineKeyboardButton[][])> GetListForm(Update update, CancellationToken ct)
    {
        var textMessage = string.Empty;
        ResultRequest<List<Form>, string> responce = null;
        InlineKeyboardButton[][] buttons = null;

        var orgId = update.CallbackQuery.Data.Split("_")[1];
        var orgOkpo = update.CallbackQuery.Data.Split("_")[2];

        try
        {
            responce = await listFormService.GetFormsById(orgId, ct);

            if (responce.Error != null)
                textMessage = responce.Error;

            if (responce.Content.Any())
            {
                textMessage = responce
                    .Content.ToDto(orgOkpo);

                buttons = new InlineKeyboardButton[][]
                {
                    new[]
                    {
                        new InlineKeyboardButton("Экспортировать",
                            $"{CallbackData.EXPORT_EXCEL_LIST_FORM}_{orgId}_{orgOkpo}")
                    }
                };
            }
            else
            {
                textMessage = TextMessage.NOT_FOUND_LIST_FORM;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            textMessage = TextMessage.INTERNAL_ERROR;
        }

        return (textMessage, buttons);
    }
}