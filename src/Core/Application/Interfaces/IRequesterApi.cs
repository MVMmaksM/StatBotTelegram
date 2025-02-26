namespace Application.Interfaces;

public interface IRequesterApi
{
    Task<HttpResponseMessage> PostAsync<T>(string requestUri, T body, CancellationToken cancellationToken);
}