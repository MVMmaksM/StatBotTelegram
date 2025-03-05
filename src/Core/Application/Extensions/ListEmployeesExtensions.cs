using Domain.Entities;

namespace Application.Extensions;

public static class ListEmployeesExtensions
{
    public static string ToDto(this List<Form> forms)
    {
        var result = string.Empty;

        if (!forms.Any())
            result = "Сотрудники не найдены!";

        result = "<b>Сотрудники:</b>\n\n";
        
        var dto = forms.Select(f =>
        {
            var res = $"Форма: {f.Okud} {f.Name} {f.PeriodicityForm.Name}\n" +
                      "Ответственные:\n";
            
            var employees =f.Employees.Select(e => 
                $"{e.FirstName} {e.LastName} {e.SurName}, тел. {ConvertPhone(e.Phone)}\n");
            
            return res + string.Join("\n", employees);
        });

        return string.Concat(result + string.Join("\n", dto));
    }

    private static string ConvertPhone(string phone)
        => $"{phone[0]}({phone[1..5]}) {phone[5..7]}-{phone[7..9]}-{phone[9..11]}";
}