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

        try
        {
            if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_INFO_ORG))
                textMessage = await GetInfoOrg(update, cancellationToken);

            if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_LIST_FORM))
                textMessage = await GetListForm(update, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            textMessage = TextMessage.INTERNAL_ERROR;
        }

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
                textMessage = TextMessage.NOT_FOUND_INFO_ORG;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            textMessage = TextMessage.INTERNAL_ERROR;
        }

        return textMessage;
    }
    
    private async Task<string> GetListForm(Update update, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        ResultRequest<List<Form>, string> responce = null;
        
        var orgId = update.CallbackQuery.Data.Split("_")[1];
        var orgOkpo = update.CallbackQuery.Data.Split("_")[2];

        try
        {
            responce = await listFormService.GetFormsById(orgId, cancellationToken);
            
            if (responce.Error != null)
                textMessage = responce.Error;

            if (responce.Content.Any())
            {
                textMessage = responce
                    .Content.
                    ToDto(orgOkpo);
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

        return textMessage;
    }
}