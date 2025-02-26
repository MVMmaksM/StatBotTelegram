namespace Application.Errors;

public class ValidateException : Exception
{
    public ValidateException(string message) : base(message)
    {
        
    }
}