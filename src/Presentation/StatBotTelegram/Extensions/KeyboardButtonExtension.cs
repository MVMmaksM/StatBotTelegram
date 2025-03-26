using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Extensions;

public static class KeyboardButtonExtension
{
    public static KeyboardButton[][] Add(this KeyboardButton[][] keyboardButtons, params KeyboardButton[] addButtons)
    {
        var buttons = keyboardButtons.ToList();
        buttons.Add(addButtons);
        return buttons.ToArray();
    }
}