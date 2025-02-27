using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram.Controllers;

public class MainMenuController(ITelegramBotClient botClient, IStateUser stateUser)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var textMessage = string.Empty;
        KeyboardButton[][] buttonMenu = null;
        
        //в зависимости от выбранной команды
        //устанавливаем сообщение, кнопки и состояние
        switch (message.Text)
        {
            case "/start":
                //устанавливаем состояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.MainMenu);
                textMessage = $"Добро пожаловать, {message.From.FirstName}!\n\n{ConstTextMessage.WelcomeText}";
                buttonMenu = KeyboradButtonMenu.ButtonsMainMenu;
                break;
            case "Поиск специалиста, ответственного за форму":
                //устанавливаем состояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.SearchEmployees);
                textMessage = ConstTextMessage.SearchEmployees;
                buttonMenu = KeyboradButtonMenu.ButtonsSearchEmployeesMenu;
                break;
            case "Получение данных о кодах статистики и перечня форм":
                //устанавливаем состояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.InfoMainMenu);
                textMessage = ConstTextMessage.SelectCommand;
                buttonMenu = KeyboradButtonMenu.ButtonsInfoCodesAndListForm;
                break;
            default:
                textMessage = ConstTextMessage.UnknownCommand;
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