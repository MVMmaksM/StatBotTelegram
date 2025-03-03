using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class MainMenuController(ITelegramBotClient botClient, ICache cache)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;

        //в зависимости от выбранной команды
        //устанавливаем сообщение, кнопки и состояние
        switch (message.Text)
        {
            //start
            case NameButton.START:
                //устанавливаем состояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.MainMenu, cancellationToken);
                textMessage = $"Добро пожаловать, {message.From.FirstName}!\n\n{TextMessage.WelcomeText}";
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                break;
            //Поиск специалиста, ответственного за форму
            case NameButton.SEARCH_EMPLOYEES:
                //устанавливаем состояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.SearchEmployees, cancellationToken);
                textMessage = TextMessage.SearchEmployees;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                break; 
            //Получение данных о кодах статистики и перечня форм
            case NameButton.GET_INFO_CODES_AND_LIST_FORMS:
                //устанавливаем состояние
                await cache.SetStateMenu(message.Chat.Id, MenuItems.InfoMainMenu, cancellationToken);
                textMessage = TextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                break;
            default:
                textMessage = TextMessage.UnknownCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                break;
        }
        
        //отправляем ответ
        await botClient.SendMessage(chatId: message.Chat.Id, 
            protectContent: true, replyParameters: message.Id,
            text: textMessage,
            parseMode: ParseMode.Html, cancellationToken: cancellationToken,
            replyMarkup: buttonMenu);
    }
}