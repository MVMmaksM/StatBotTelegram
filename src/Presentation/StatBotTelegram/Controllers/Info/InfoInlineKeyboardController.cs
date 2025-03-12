using Application.Constants;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class InfoInlineKeyboardController(
    ITelegramBotClient botClient,
    IInfoOrganization infoOrgService,
    IListForm listFormService)
{
    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;

        if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_INFO_ORG))
            textMessage = await GetInfoOrg(update, cancellationToken);

        if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_LIST_FORM))
            textMessage = await GetListForm(update, cancellationToken);

        await botClient.SendMessage(chatId: update.CallbackQuery.From.Id,
            protectContent: false,
            text: textMessage,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken);
    }

    private async Task<string> GetInfoOrg(Update update, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        ResultRequest<List<InfoOrganization>, ErrorInfoOrganization> responce = null;
        
        var okpo = update.CallbackQuery.Data.Split("_")[1];
        var request = new RequestInfoForm()
        {
            Okpo = okpo
        };

        try
        {
            responce = await infoOrgService.GetInfoOrganization(request, cancellationToken);
            
            if (responce.Error != null)
                textMessage = responce.Error.ToDto();

            if (responce.Content.Any())
            {
                textMessage = responce
                    .Content
                    .Where(o => o.Okpo == okpo)
                    .ToList()
                    .ToFullDto();
            }
            else
            {
                textMessage = "Информация по организации не найдена!";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            textMessage = "Произогла внутренняя ошибка";
        }

        return textMessage;
    }
    
    private async Task<string> GetListForm(Update update, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        ResultRequest<List<Form>, string> responce = null;
        
        var orgId = update.CallbackQuery.Data.Split("_")[1];

        try
        {
            responce = await listFormService.GetFormsById(orgId, cancellationToken);
            
            if (responce.Error != null)
                textMessage = responce.Error;

            if (responce.Content.Any())
            {
                textMessage = responce
                    .Content.
                    ToDto();
            }
            else
            {
                textMessage = "Формы не найдены!";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            textMessage = "Произошла внутренняя ошибка";
        }

        return textMessage;
    }
}