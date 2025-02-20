using Application.Interfaces;
using Application.Services;
using StatBotTelegram.Components;
using StatBotTelegram.Controllers;
using Telegram.Bot;

namespace StatBotTelegram.Extensions;

public static class ServiceCollectionsExtension
{
    public static HostApplicationBuilder AddAppServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITelegramBotClient>
            (provider => new TelegramBotClient(builder.Configuration.GetSection("ApiTokenBot")?.Value));
   
        builder.Services.AddTransient<StartMenuHandleController>();
        builder.Services.AddTransient<SearchEmployeesHandleController>();
        builder.Services.AddTransient<InfoCodesAndListFormsHandleController>();

        builder.Services.AddSingleton<IStateUser, StorageStateUser>();
        
        builder.Services.AddHostedService<TelegramBot>();
        return builder;
    }
}