using System.Net.Http.Headers;
using Application.Interfaces;
using Application.Models;
using Newtonsoft.Json;

namespace Application.Services;

public class RequesterApiService(HttpClient httpClient) : IRequesterApi
{
    public async Task<ResultRequest<TContent, TError>> PostAsync<TBody, TContent, TError>
        (string requestUri, TBody body, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonConvert.SerializeObject(body));
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;
        HttpResponseMessage responce = null;
        var result = new ResultRequest<TContent, TError>();

        try
        {
            responce = await httpClient.SendAsync(request, cancellationToken);
            if (responce.IsSuccessStatusCode)
            {
                var dataResponce = await responce.Content.ReadAsStringAsync();
                result.Content = JsonConvert
                    .DeserializeObject<TContent>(dataResponce);
            }
            else if (responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var dataResponce = await responce.Content.ReadAsStringAsync();
                result.Error = JsonConvert
                    .DeserializeObject<TError>(dataResponce);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return result;
    }

    public async Task<ResultRequest<TContent, TError>> GetAsync<TContent, TError>(string requestUri,
        CancellationToken cancellationToken)
    {
        //var content = new StringContent(JsonConvert.SerializeObject(body));
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        /*content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;*/
        HttpResponseMessage responce = null;

        try
        {
            responce = await httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        var result = new ResultRequest<TContent, TError>();

        if (responce.IsSuccessStatusCode)
        {
            var dataResponce = await responce.Content.ReadAsStringAsync();
            result.Content = JsonConvert
                .DeserializeObject<TContent>(dataResponce);
        }
        else if (responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var dataResponce = await responce.Content.ReadAsStringAsync();
            result.Error = JsonConvert
                .DeserializeObject<TError>(dataResponce);
        }

        return result;
    }
}