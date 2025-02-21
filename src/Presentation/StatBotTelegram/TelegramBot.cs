using Application.Constants;
using Application.Interfaces;
using StatBotTelegram.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StatBotTelegram;

public class TelegramBot(ITelegramBotClient telegramBotClient, 
    StartMenuHandleController startMenuHandleController,
    SearchEmployeesHandleController searchEmployeesHandleController,
    InfoCodesAndListFormsHandleController infoCodesAndListFormsHandleController,
    InfoOrganizationHandleController infoOrganizationHandleController,
    IStateUser stateUser) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await telegramBotClient.ReceiveAsync(
            errorHandler: HandleErrorAsync, 
            cancellationToken: stoppingToken, 
            receiverOptions: new ReceiverOptions() {AllowedUpdates = {}}, 
            updateHandler:HandleUpdateAsync);
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient,Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
        {
            var state = stateUser.GetState(update.Message.Chat.Id);
            if (state is null || state.MenuItem == MenuItems.MainMenu || update.Message.Text == "/start")
            {
                await startMenuHandleController.Handle(update.Message, cancellationToken);
                return; 
            }
                
            switch (state.MenuItem)
            {
                //если в меню поиска сотрудников
                case MenuItems.SearchEmployees:
                    await searchEmployeesHandleController.Handle(update.Message, cancellationToken);
                    break;
                //если в меню получения кодов статистики и перечня форм
                case MenuItems.GetInfoCodesAndListForm:
                    await infoCodesAndListFormsHandleController.Handle(update.Message, cancellationToken);
                    break;
                //получение данных организации
                case MenuItems.GetInfoOrganization:
                    await infoOrganizationHandleController.Handle(update.Message, cancellationToken);
                    break;
                //получение перечня форм
                case MenuItems.GetListForm:
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

