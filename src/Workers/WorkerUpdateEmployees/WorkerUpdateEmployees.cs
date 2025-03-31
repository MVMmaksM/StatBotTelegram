using WorkerUpdateEmployees.Interfaces;

namespace WorkerUpdateEmployees;

public class WorkerUpdateEmployees
    (IWebRequester webRequester, IParser parser) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var content = await webRequester.GetContentAsync("https://66.rosstat.gov.ru/contacts");
        
        if(content == null)
            throw new Exception("Пустой content");
        
        parser.ParseFormEmployee(content);
    }
}