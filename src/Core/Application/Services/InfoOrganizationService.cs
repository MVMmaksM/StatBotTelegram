using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Interfaces;
using Application.Models;
using Newtonsoft.Json;

namespace Application.Services;

public class InfoOrganizationService(HttpClient httpClient) : IInfoOrganizationService
{
    public async Task<string> GetInfoOrganization(FilterOrganization filter, CancellationToken ct)
    {
        string result = String.Empty;
        
        var content = new StringContent(JsonConvert.SerializeObject(filter));
        var request = new HttpRequestMessage(HttpMethod.Post, "/webstat/api/gs/organizations");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;
        var responce = await httpClient.SendAsync(request, ct);
        
        if (responce.IsSuccessStatusCode)
        {
            var infoStr = await responce.Content.ReadAsStringAsync();
            var infoOrg = JsonConvert.DeserializeObject<InfoOrganization>(infoStr);
            result = $"<b>Данные о кодах статистики:</b>\n\n" +
                     $"краткое наименование: {infoOrg.ShortName}\n" +
                     $"ОКПО / Идентификационный номер ТОСП: {infoOrg.Okpo}\n" +
                     $"ОГРН / ОГРНИП: {infoOrg.Ogrn}\n" +
                     $"Дата регистрации: {infoOrg.DateReg}\n" +
                     $"ИНН: {infoOrg.Inn}\n" +
                     $"ОКАТО фактический: {infoOrg.OkatoFact}\n" +
                     $"ОКАТО регистрации: {infoOrg.OkatoReg}\n" +
                     $"ОКТМО фактический: {infoOrg.OktmoFact}\n" +
                     $"ОКТМО регистрации: {infoOrg.OktmoReg}\n" +
                     $"ОКОГУ: {infoOrg.Okogu}\n" +
                     $"ОКФС: {infoOrg.Okfs}\n" +
                     $"ОКОПФ: {infoOrg.Okopf}\n";
        }
        else if (responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            result = await responce.Content.ReadAsStringAsync();
        }
        else
        {
            result = "Сервис получения данных временно недоступен!";
        }

        return result;
    }
}