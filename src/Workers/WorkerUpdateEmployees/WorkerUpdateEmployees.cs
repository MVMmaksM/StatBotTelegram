using Newtonsoft.Json;
using WorkerUpdateEmployees.Extensions;
using WorkerUpdateEmployees.Interfaces;
using WorkerUpdateEmployees.Model;

namespace WorkerUpdateEmployees;

public class WorkerUpdateEmployees
    (IWebRequester webRequester, 
        IParser parser,
        IRepository repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var content = await webRequester.GetContentAsync("https://66.rosstat.gov.ru/storage/mediabank/contakt.csv");
        
            if(content == null)
                throw new Exception("Пустой content");
        
            var contacts = parser.ParseContact(content);
            var forms = contacts.GetForms();
            
            var formsJson = JsonConvert.SerializeObject(forms);
            await repository.UpdateContactsAsync(formsJson);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}