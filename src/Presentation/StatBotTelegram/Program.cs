using StatBotTelegram.Extensions;

namespace StatBotTelegram;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddAppServices();

        var host = builder.Build();
        await host.RunAsync();
    }
}