using Application.Models;

namespace Application.Extensions;

public static class ErrorApiInfoOrganizationExtension
{
    public static string ToDto(this ErrorInfoOrganization error)
    {
        return $"Ошибка при выполнении запроса: {error.Message}";
    }
}