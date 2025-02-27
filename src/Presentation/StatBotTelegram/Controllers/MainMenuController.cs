using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StatBotTelegram.Controllers;

public class MainMenuController(ITelegramBotClient botClient, IStateUser stateUser)
{
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "/start":
                //устанавливаем состояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.MainMenu);
                //отправляем ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"Добро пожаловать, {message.From.FirstName}!\n\n{ConstTextMessage.WelcomeText}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsMainMenu);
                break;
            case "Поиск специалиста, ответственного за форму":
                //устанавливаем состояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.SearchEmployees);
                //отправляем ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true,
                    replyParameters: message.Id,
                    text: $"{ConstTextMessage.SearchEmployees}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsSearchEmployeesMenu);
                break;
            case "Получение данных о кодах статистики и перечня форм":
                //устанавливаем состояние
                stateUser.SetStateMenu(message.Chat.Id, MenuItems.GetInfoCodesAndListForm);
                //отправляем ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true,
                    replyParameters: message.Id,
                    text: $"{ConstTextMessage.SelectCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsInfoCodesAndListForm);
                break;
            default:
                //отправляем ответ
                await botClient.SendMessage(chatId: message.Chat.Id, 
                    protectContent: true, replyParameters: message.Id,
                    text: $"{ConstTextMessage.UnknownCommand}",
                    parseMode: ParseMode.Html, cancellationToken: cancellationToken,
                    replyMarkup: KeyboradButtonMenu.ButtonsMainMenu);
                break;
        }
    }
}