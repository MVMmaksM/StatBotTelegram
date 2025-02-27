using Application.Interfaces;
using Application.Models;
using Application.Services;
using FluentValidation;
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
   
        builder.Services.AddTransient<MainMenuController>();
        builder.Services.AddTransient<SearchEmployeesController>();
        builder.Services.AddTransient<InfoMainMenuController>();
        builder.Services.AddTransient<InfoOrganizationController>();
        builder.Services.AddTransient<ListFormController>();
        
        builder.Services.AddTransient<IInfoOrganizationService, InfoOrganizationService>();
        builder.Services.AddTransient<IListForm, ListFormService>();
   
        builder.Services.AddSingleton<IStateUser, StorageStateUser>();
        
        builder.Services.AddHttpClient<IRequesterApi, RequesterApi>(clientConfigure =>
        {
            clientConfigure.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddressWebsbor")?.Value);
        });

        builder.Services.AddValidatorsFromAssemblyContaining<RequestInfoForm>(ServiceLifetime.Transient);
        builder.Services.AddHostedService<TelegramBot>();
        
        return builder;
    }
}