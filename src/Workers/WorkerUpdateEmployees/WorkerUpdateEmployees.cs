using Newtonsoft.Json;
using WorkerUpdateEmployees.Extensions;
using WorkerUpdateEmployees.Interfaces;
using WorkerUpdateEmployees.Model;

namespace WorkerUpdateEmployees;

public class WorkerUpdateEmployees(
    IWebRequester webRequester,
    IParser parser,
    IRepository repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                Console.WriteLine("Старт метода: updating employees");
                await RunServicesAsync(stoppingToken);
            }
            catch (OperationCanceledException error)
            {
                Console.WriteLine(error.Message);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private async Task RunServicesAsync(object obj)
    {
        var content = await webRequester.GetContentAsync("https://66.rosstat.gov.ru/storage/mediabank/contakt.csv");

        if (content == null)
            throw new Exception("Пустой content");

        var contactsDto = parser.ParseContact(content);
        var contacts = contactsDto.GetContacts();

        var contactsJson = JsonConvert.SerializeObject(contacts);
        var code = await repository.UpdateContactsAsync(contactsJson);

        Console.WriteLine($"{code}");
    }
}