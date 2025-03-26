using Application.Models.Templates;

namespace Application.Extensions;

public static class TemplatesExtensions
{
    public static string ToDto(this List<Template> templates)
    {
        var dto = templates.Select(t => 
            $"Название: {t.Name}\n" +
            $"Код: {t.Code}\n" +
            $"Версия: {t.Version}\n");
        
        return string.Join("\n", dto);
    }
}