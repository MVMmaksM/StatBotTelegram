using System.Net;
using Microsoft.EntityFrameworkCore;
using WorkerUpdateEmployees.Data;
using WorkerUpdateEmployees.Interfaces;
using WorkerUpdateEmployees.Services;

namespace WorkerUpdateEmployees.Extensions;

public static class ServiceCollectionsExtension
{
    public static HostApplicationBuilder AddAppServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddHostedService<WorkerUpdateEmployees>();
        builder.Services.AddHttpClient<IWebRequester, WebRequester>();
        builder.Services.AddTransient<IRepository, Repository>();
        builder.Services.AddTransient<IParser, Parser>();
        return builder;
    }
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