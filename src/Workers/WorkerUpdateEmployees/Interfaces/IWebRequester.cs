namespace WorkerUpdateEmployees.Interfaces;

public interface IWebRequester
{
    Task<string> GetContentAsync(string url);
}