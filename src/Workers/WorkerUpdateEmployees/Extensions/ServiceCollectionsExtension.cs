using System.Net;
using WorkerUpdateEmployees.Interfaces;
using WorkerUpdateEmployees.Services;

namespace WorkerUpdateEmployees.Extensions;

public static class ServiceCollectionsExtension
{
    public static HostApplicationBuilder AddAppServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<WorkerUpdateEmployees>();
        builder.Services.AddHttpClient<IWebRequester, WebRequester>();
        builder.Services.AddTransient<IParser, Parser>();
        return builder;
    }
}