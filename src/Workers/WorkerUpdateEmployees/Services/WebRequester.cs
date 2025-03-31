using WorkerUpdateEmployees.Interfaces;

namespace WorkerUpdateEmployees.Services;

public class WebRequester(HttpClient httpClient) : IWebRequester
{
    public async Task<string> GetContentAsync(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var responce = await httpClient.SendAsync(request);
        return await responce.Content.ReadAsStringAsync();
    }
}