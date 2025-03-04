using Application.Interfaces;
using Application.Models;
using Application.Services;
using FluentValidation;
using StatBotTelegram.Components;
using StatBotTelegram.Controllers;
using Telegram.Bot;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persisitence.Database;

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
   
        builder.Services.AddSingleton<ICache, CacheRedis>();
        
        builder.Services.AddHttpClient<IRequesterApi, RequesterApi>(clientConfigure =>
        {
            clientConfigure.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddressWebsbor")?.Value);
        });
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetSection("RedisCache")?.Value;
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { options.Configuration }
            };
        });

        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddValidatorsFromAssemblyContaining<RequestInfoForm>(ServiceLifetime.Transient);
        builder.Services.AddHostedService<TelegramBot>();
        
        return builder;
    }

    /// <summary>
    /// для применения миграций
    /// </summary>
    /// <param name="host"></param>
    public static void ExecuteMigrate(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();
            if(dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();
        }
    }
}