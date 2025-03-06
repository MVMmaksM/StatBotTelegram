using Application.Extensions;
using Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class InfoInlineKeyboardController(ITelegramBotClient botClient, IDistributedCache cacheRedis)
{
    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var splitted = update.CallbackQuery.Data.Split("_");
        var infoOrgStr = await cacheRedis.GetStringAsync($"infoOkpo_{splitted[1]}");
        var infoOrg = JsonConvert.DeserializeObject<InfoOrganization>(infoOrgStr);
        
        await botClient.SendMessage(chatId: update.CallbackQuery.From.Id,
            protectContent: false, 
            text: infoOrg.ToOneDto(),
            parseMode: ParseMode.Html, cancellationToken: cancellationToken);
    }
}