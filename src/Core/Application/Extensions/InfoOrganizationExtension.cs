using System.Text;
using Application.Models;

namespace Application.Extensions;

public static class InfoOrganizationExtension
{
    public static string ToFullDto(this List<InfoOrganization> organization)
    {
        var fullInfo = organization
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

        return string.Join("\n", fullInfo);
    }

    public static string ToShortDto(this List<InfoOrganization> organization)
    {
        var shortInfo = organization.Select(info =>
            $"ОКПО / Идентификационный номер ТОСП: {info.Okpo}\n" +
            $"Краткое наименование: {info.ShortName}\n" +
            $"ОКАТО фактический: {info.OkatoFact.Code} - {info.OkatoFact.Name}\n");

        return string.Join("\n", shortInfo);
    }

    public static string ToOneDto(this InfoOrganization organization)
    {
        var fullInfo =
            "<b>Данные о кодах статистики:</b>\n\n" +
            $"Краткое наименование: {organization.ShortName}\n" +
            $"ОКПО / Идентификационный номер ТОСП: {organization.Okpo}\n" +
            $"ОГРН / ОГРНИП: {organization.Ogrn}\n" +
            $"Дата регистрации: {organization.DateReg}\n" +
            $"ИНН: {organization.Inn}\n" +
            $"ОКАТО фактический: {organization.OkatoFact.Code} - {organization.OkatoFact.Name}\n" +
            $"ОКАТО регистрации: {organization.OkatoReg.Code} - {organization.OkatoReg.Name}\n" +
            $"ОКТМО фактический: {organization.OktmoFact.Code} - {organization.OktmoFact.Name}\n" +
            $"ОКТМО регистрации: {organization.OktmoReg.Code} - {organization.OktmoReg.Name}\n" +
            $"ОКОГУ: {organization.Okogu.Code} - {organization.Okogu.Name}\n" +
            $"ОКФС: {organization.Okfs.Code} - {organization.Okfs.Name}\n" +
            $"ОКОПФ: {organization.Okopf.Code} - {organization.Okopf.Name}\n";


        return fullInfo;
    }
}