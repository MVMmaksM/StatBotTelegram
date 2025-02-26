using System.Text;
using Application.Models;

namespace Application.Extensions;

public static class InfoOrganizationExtension
{
    public static string ToDto(this List<InfoOrganization> organization)
    {
        if (organization.Count == 0)
            return "Организация не найдена!";
        
        var dto = organization
            .Select(info =>
                "<b>Данные о кодах статистики:</b>\n\n" +
                $"Краткое наименование: {info.ShortName}\n" +
                $"ОКПО / Идентификационный номер ТОСП: {info.Okpo}\n" +
                $"ОГРН / ОГРНИП: {info.Ogrn}\n" +
                $"Дата регистрации: {info.DateReg}\n" +
                $"ИНН: {info.Inn}\n" +
                $"ОКАТО фактический: {info.OkatoFact.Code} - {info.OkatoFact.Name}\n" +
                $"ОКАТО регистрации: {info.OkatoReg.Code} - {info.OkatoReg.Name}\n" +
                $"ОКТМО фактический: {info.OktmoFact.Code} - {info.OktmoFact.Name}\n" +
                $"ОКТМО регистрации: {info.OktmoReg.Code} - {info.OktmoReg.Name}\n" +
                $"ОКОГУ: {info.Okogu.Code} - {info.Okogu.Name}\n" +
                $"ОКФС: {info.Okfs.Code} - {info.Okfs.Name}\n" +
                $"ОКОПФ: {info.Okopf.Code} - {info.Okopf.Name}\n"
            );

        return string.Join("\n", dto);
    }
}