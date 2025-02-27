namespace Application.Interfaces;

public interface IRequesterApi
{
    Task<HttpResponseMessage> PostAsync<T>(string requestUri, T body, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
}