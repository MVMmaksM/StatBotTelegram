namespace Application.Models;

public class ResultRequest<TContent, TError>
{
    public TContent Content { get; set; } = default(TContent);
    public TError? Error { get; set; } = default(TError);
}