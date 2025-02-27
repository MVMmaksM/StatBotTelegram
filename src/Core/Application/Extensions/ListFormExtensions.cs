using Application.Models;

namespace Application.Extensions;

public static class ListFormExtensions
{
    public static string ToDto(this List<Form> forms)
    {
        if (forms.Count == 0)
            return "Формы не найдены!";

        var result = "<b>Перечень форм:</b>\n\n";

        var dto = forms.Select(f =>
            $"Форма: {f.Index}\n" +
            $"Название: {f.Name}\n");

        return string.Concat(result, string.Join("\n", dto));
    }
}