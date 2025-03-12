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
    IDistributedCache cacheRedis,
    IInfoOrganization infoOrgService)
{
    public async Task Handle(Update update, CancellationToken cancellationToken)
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

        await botClient.SendMessage(chatId: update.CallbackQuery.From.Id,
            protectContent: false,
            text: textMessage,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken);
    }
}