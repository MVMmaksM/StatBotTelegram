using System.Net.Http.Headers;
using Application.Interfaces;
using Newtonsoft.Json;

namespace Application.Services;

public class RequesterApi(HttpClient httpClient) : IRequesterApi
{
    public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T body, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonConvert.SerializeObject(body));
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;
        return await httpClient.SendAsync(request, cancellationToken);
    }
}