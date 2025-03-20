using Application.Models;

namespace Application.Extensions;

public static class ListFormExtensions
{
    public static string ToDto(this List<Form> forms, string orgOkpo = null)
    {
        var result = orgOkpo is null ? 
            "<b>Перечень форм:</b>\n\n" : 
            $"<b>Перечень форм для ОКПО {orgOkpo}:</b>\n\n";

        var dto = forms.Select(f =>
            $"Индекс формы: {f.Index}\n" +
            $"Периодичность формы: {f.FormPeriod}\n" +
            $"ОКУД: {f.Okud}\n");

        return string.Concat(result, string.Join("\n", dto));
    }
}