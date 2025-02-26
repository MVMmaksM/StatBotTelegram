using FluentValidation.Results;

namespace Application.Extensions;

public static class ValidationResultExtensions
{
    public static string ToDto(this List<ValidationFailure> errors)
    {
        var dto = errors
            .Select(e => e.ErrorMessage + "\n");

        return string.Join("\n", dto);
    }
}