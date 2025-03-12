using Application.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Helpers;

public static class CreateInlineKeyboardButtonInfoOrg
{
    public static InlineKeyboardButton[][] Create<T>
    (
        List<T> objects,
        string nameCallbackData,
        string propertyForCallbackData,
        string textForButton,
        string propertyForTextButton = null)
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

            var identifierCallbackData = typeof(T)
                .GetProperty(propertyForCallbackData)
                .GetValue(objects[i], null)
                .ToString();

            inlineButtons[i] = new InlineKeyboardButton[]
            {
                new InlineKeyboardButton
                {
                    Text = textButton,
                    CallbackData = $"{nameCallbackData}{identifierCallbackData}",
                }
            };
        }

        return inlineButtons;
    }
}