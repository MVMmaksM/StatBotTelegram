using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Extensions;

public static class InlineKeyboardButtonExtensions
{
    public static InlineKeyboardButton[][] AddButton(this InlineKeyboardButton[][] inlineButtons, InlineKeyboardButton button)
    {
        var buttons = inlineButtons.ToList();
        buttons.Add(new[] { button });
        return buttons.ToArray();
    }
}