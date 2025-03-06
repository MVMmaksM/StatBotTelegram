using Application.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Helpers;

public static class CreateInlineKeyboardButtonInfoOrg
{
    public static InlineKeyboardButton[][] Create(List<InfoOrganization> infoOrg)
    {
        var inlineButtons = new InlineKeyboardButton[infoOrg.Count][];

        for (int i = 0; i < infoOrg.Count(); i++)
        {
            inlineButtons[i] = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton
                {
                    Text = infoOrg[i].Okpo,
                    CallbackData = $"getInfoOrg_{infoOrg[i].Okpo}",
                }
            };
        }

        return inlineButtons;
    }
}