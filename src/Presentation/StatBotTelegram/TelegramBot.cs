using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StatBotTelegram;

public class TelegramBot(
    ITelegramBotClient telegramBotClient,
    MainMenuController mainMenuController,
    SearchEmployeesController searchEmployeesController,
    InfoMainMenuController infoMainMenuController,
    InfoOrganizationController infoOrganizationController,
    ListFormController listFormController,
    InfoInlineKeyboardController infoInlineKeyboardController,
    ICache cache) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await telegramBotClient.ReceiveAsync(
            errorHandler: HandleErrorAsync,
            cancellationToken: stoppingToken,
            receiverOptions: new ReceiverOptions() { AllowedUpdates = { } },
            updateHandler: HandleUpdateAsync);
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            if (update.CallbackQuery.Data.StartsWith(CallbackData.GET_INFO_ORG) || 
                update.CallbackQuery.Data.StartsWith(CallbackData.GET_LIST_FORM))
            {
                await infoInlineKeyboardController.Handle(update, cancellationToken);
            }
        }

        if (update.Type == UpdateType.Message)
        {
            var state = await cache.GetUserState(update.Message.Chat.Id, cancellationToken);
            if (state is null || state.MenuItem == MenuItems.MainMenu || update.Message.Text == "/start")
            {
                await mainMenuController.Handle(update.Message, cancellationToken);
                return;
            }

            switch (state.MenuItem)
            {
                //если в меню поиска сотрудников
                case MenuItems.SearchEmployees:
                    await searchEmployeesController.Handle(update.Message, cancellationToken);
                    break;
                //если в меню получения кодов статистики и перечня форм
                case MenuItems.InfoMainMenu:
                    await infoMainMenuController.Handle(update.Message, cancellationToken);
                    break;
                //получение данных организации
                case MenuItems.GetInfoOrganization:
                    await infoOrganizationController.Handle(update.Message, cancellationToken);
                    break;
                //получение перечня форм
                case MenuItems.GetListForm:
                    await listFormController.Handle(update.Message, cancellationToken);
                    break;
            }
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        Console.WriteLine("Waiting 10 seconds before retry");
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}