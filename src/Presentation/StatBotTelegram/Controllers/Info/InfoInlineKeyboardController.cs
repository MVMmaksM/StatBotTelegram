using System.Text;
using System.Xml;
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
    IAbstractFactoryGenFile factoryGenFile,
    ITemplateService templateService)
{
    public async Task Handle(Update update, CancellationToken ct)
    {
        InlineKeyboardButton[][] buttons = null;
        var fileName = string.Empty;
        var textMessage = string.Empty;
        byte[] bytesFile = null;

        try
        {
            //скачивание шаблонов
            if (update.CallbackQuery.Data.StartsWith(CallbackData.DOWNLOAD_TEMPLATE))
                (textMessage, fileName, bytesFile) = await DownloadTemplate(update, ct);
            //инфа об организации
            if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_INFO_ORG))
                (textMessage, buttons) = await GetInfoOrg(update, ct);
            //перечень форм
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
    private async Task<(string, string, byte[]?)> DownloadTemplate(Update update, CancellationToken ct)
    {
        var fileName = string.Empty;
        var textMessage = string.Empty;
        byte[] bytesFile = null;

        var templateId = update.CallbackQuery.Data.Split("_")[1];
        var responceGuidTemplate = await templateService.GetGuidByTemplateId(templateId, ct);

        if (responceGuidTemplate.Error != null)
            textMessage = responceGuidTemplate.Error;

        if (responceGuidTemplate.Content == null)
        {
            textMessage = TextMessage.INTERNAL_ERROR;
        }
        else
        {
            var templateGuid = responceGuidTemplate.Content;
            var responceTemplate = await templateService.DownloadTemplateByGiud(templateGuid, ct);

            if (responceTemplate.Error != null)
                textMessage = responceTemplate.Error;

            if (responceTemplate.Content == null)
            {
                textMessage = TextMessage.INTERNAL_ERROR;
            }
            else
            {
                bytesFile = Encoding.UTF8.GetBytes(responceTemplate.Content);
                fileName = await GetFileNameByTemplate(responceTemplate.Content);
            }
        }
        
        return (textMessage, fileName, bytesFile);
    }

    private async Task<string> GetFileNameByTemplate(string template)
    {
        //путь к директории для хранения 
        //временных файлов шаблонов
        var pathTempDirectory = Path.Combine(Environment.CurrentDirectory, "temp");
        //полный путь к файлу шаблона
        var fullnameFile = Path.Combine(pathTempDirectory, $"tmp_{DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_ff")}.xml");
        
        //если директории не существует
        //то создаем ее
        if(!Directory.Exists(pathTempDirectory))
            Directory.CreateDirectory(pathTempDirectory);
        
        //пишем в файл шаблона
        await File.WriteAllTextAsync(fullnameFile, template);
        
        var xmlTemplate = new XmlDocument();
        //читаем xml шаблон
        xmlTemplate.Load(fullnameFile);
        var root = xmlTemplate.DocumentElement;
        //получаем значение атрибута code
        var code = root.Attributes["code"].Value;
        //получаем значение атрибута version
        var version = root.Attributes["version"].Value;

        if (code == null || version == null)
            throw new Exception("Не найден код или версия шаблона!");
        
        //удаляемп временный файл шаблона
        File.Delete(fullnameFile);
        
        //возвращаем имя файла в виде code_version.xml
        return  $"{code}_{version}.xml";
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