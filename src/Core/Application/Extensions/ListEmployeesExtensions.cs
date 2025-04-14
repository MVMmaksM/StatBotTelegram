using Domain.Entities;

namespace Application.Extensions;

public static class ListEmployeesExtensions
{
    public static string ToDto(this List<Form> forms)
    {
        var result = string.Empty;

        if (!forms.Any())
            result = "Сотрудники не найдены!";

        var dto = forms.Select(f =>
        {
            var res = $"Форма: {f.Okud} {f.Name}\n\n" +
                      "Ответственные:\n";

            /*var employees = f.Employees.Select(e =>
                $"{e.LastName} {e.FirstName} {e.SurName}, тел. {ConvertPhone(e.Phone)}\n");*/

            return res; /*+ string.Join("\n", employees);*/
        });

        return string.Join("\n", dto);
    }

    private static string ConvertPhone(string phone)
        => $"8 ({phone[0..4]}) {phone[4..6]}-{phone[6..8]}-{phone[8..10]}";
};
