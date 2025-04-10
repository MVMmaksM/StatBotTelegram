using Application.Interfaces;
using Application.Models;
using Application.Models.SearchEmployees;
using Application.Services;
using Application.Services.FileGen;
using FluentValidation;
using StatBotTelegram.Components;
using StatBotTelegram.Controllers;
using Telegram.Bot;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

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
        builder.Services.AddTransient<InfoInlineKeyboardController>();
        
        builder.Services.AddTransient<IInfoOrganization, InfoOrganizationService>();
        builder.Services.AddTransient<IListForm, ListFormService>();
        builder.Services.AddTransient<ISearchEmployees, SearchEmployeesService>();
        builder.Services.AddTransient<IAbstractFactoryGenFile, FileGenFactory>();
        builder.Services.AddTransient<ITemplateService, TemplateService>();
   
        builder.Services.AddSingleton<ICache, CacheRedisService>();
        
        builder.Services.AddHttpClient<IRequesterApi, RequesterApiService>(clientConfigure =>
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

        builder.Services.AddDbContext<AppDbContext>(ServiceLifetime.Singleton);
        builder.Services.AddValidatorsFromAssemblyContaining<RequestInfoForm>(ServiceLifetime.Transient);
        builder.Services.AddValidatorsFromAssemblyContaining<RequestGetTemplate>(ServiceLifetime.Transient);
        //builder.Services.AddValidatorsFromAssemblyContaining<RequestSearchEmployees>(ServiceLifetime.Transient);
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