using Application.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Helpers;

public static class CreatorInlineKeyboardButton
{
    public static InlineKeyboardButton[][] CreateFromList<T>
    (
        List<T> objects,
        string nameCallbackData,
        string textForButton,
        string propertyForTextButton = null,
        params string[] propertyForCallbackData)
    {
        var inlineButtons = new InlineKeyboardButton[objects.Count][];
        var textButton = String.Empty;

        for (int i = 0; i < objects.Count(); i++)
        {
            if (propertyForTextButton != null)
            {
                textButton = typeof(T)
                    .GetProperty(propertyForTextButton)
                    .GetValue(objects[i], null)
                    .ToString();
            }
            else
            {
                textButton = textForButton;
            }

            var identifierCallbackData = string.Join("_", propertyForCallbackData
                .Select(p =>
                    typeof(T)
                        .GetProperty(p)
                        .GetValue(objects[i], null)));

            inlineButtons[i] = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton
                {
                    Text = textButton,
                    CallbackData = $"{nameCallbackData}_{identifierCallbackData}",
                }
            };
        }

        return inlineButtons;
    }
}