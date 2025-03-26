using Application.Models;

namespace Application.Interfaces;

public interface IRequesterApi
{
    Task<ResultRequest<TContent, TError>> PostAsync<TBody, TContent, TError>
        (string requestUri, TBody body, CancellationToken cancellationToken);

    Task<ResultRequest<TContent, TError>> GetAsync<TContent, TError>(string requestUri,
        CancellationToken cancellationToken);
    
    Task<ResultRequest<TContent, TError>> PutAsync<TBody, TContent, TError>
        (string requestUri, TBody body, CancellationToken cancellationToken);
}