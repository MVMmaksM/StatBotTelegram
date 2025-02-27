using Application.Models;

namespace Application.Extensions;

public static class ListFormExtensions
{
    public static string ToDto(this List<Form> forms)
    {
        if(forms.Count == 0)
            return "Формы не найдены!";

        var dto = forms.Select(f => 
            "<b>Перечень форм:</b>\n\n" +
            $"Форма: {f.Index}\n" +
            $"Название: {f.Name}\n");
        
        return string.Join("\n", dto);
    }
}