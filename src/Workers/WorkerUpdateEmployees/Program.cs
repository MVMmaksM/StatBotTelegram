using WorkerUpdateEmployees.Extensions;

namespace WorkerUpdateEmployees;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddAppServices();

        var host = builder.Build();
        host.ExecuteMigrate();
        await host.RunAsync();
    }
}