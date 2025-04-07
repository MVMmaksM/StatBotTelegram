using System.Net;
using Persistence.Database;
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
        builder.Services.AddTransient<IParser, Parser>();
        return builder;
    }
}