using Application.Interfaces;
using Application.Models;
using OfficeOpenXml;

namespace Application.Services.FileGen;

//конкретный генератор Excel файлов
public class ExcelFileGen : IExcelFileGen
{
    private const string OKPO = "ОКПО / Идентификационный номер ТОСП";
    private const string OGRN = "ОГРН / ОГРНИП";
    private const string DATE_REG = "Дата регистрации";
    private const string INN = "ИНН";
    private const string OKATO_FACT = "ОКАТО фактический";
    private const string OKATO_REG = "ОКАТО регистрации";
    private const string OKTMO_FACT = "ОКТМО фактический";
    private const string OKTMO_REG = "ОКТМО регистрации";
    private const string OKOGU = "ОКОГУ";
    private const string OKFS = "ОКФС";
    private const string OKOPF = "ОКОПФ";
    private const string FORM_INDEX = "Индекс формы";
    private const string FORM_NAME = "Наименование формы";
    private const string FORM_PERIOD = "Периодичность формы";
    private const string FORM_END_TIME = "Срок сдачи формы";
    private const string FORM_REPORTED_PERIOD = "Отчетный период";
    private const string FORM_COMMENT = "Комментарий";
    private const string FORM_OKUD = "ОКУД";

    public async Task<byte[]> GetFileListForm(List<Form> forms, string okpo, CancellationToken ct)
    {
        const int COUNT_SKIP_ROW = 4;
        
        using (var package = new ExcelPackage())
        {
            var sheet = package.Workbook.Worksheets.Add("Sheet1");
            
            //перенос текста
            sheet.Cells[3, 1, forms.Count() + COUNT_SKIP_ROW, 11].Style.WrapText = true;
            //горизонтальное выравнивание
            sheet.Cells[1,1, forms.Count() + COUNT_SKIP_ROW, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //вертикальное выравнивание
            sheet.Cells[1,1, forms.Count() + COUNT_SKIP_ROW, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //
            sheet.Cells["A1:G1"].Merge = true;
            sheet.Cells["A1:G1"].Value = $"Перечень форм для ОКПО: {okpo}";
            sheet.Cells["A1:G1"].Style.Font.Bold = true;
            sheet.Cells["A1:G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //дата формирования
            sheet.Cells["A2:G2"].Merge = true;
            sheet.Cells["A2:G2"].Value = $"Дата формирования - {DateTime.Now.ToShortDateString()}";
            //шапка данных
            //индекс формы
            sheet.Cells["A3"].Value = FORM_INDEX;
            sheet.Cells["A3"].Style.Font.Bold = true;
            //наименование формы
            sheet.Cells["B3"].Value = FORM_NAME;
            sheet.Cells["B3"].Style.Font.Bold = true;
            //периодичность формы
            sheet.Cells["C3"].Value = FORM_PERIOD;
            sheet.Cells["C3"].Style.Font.Bold = true;
            //Срок сдачи формы
            sheet.Cells["D3"].Value = FORM_END_TIME;
            sheet.Cells["D3"].Style.Font.Bold = true;
            //Отчетный период
            sheet.Cells["E3"].Value = FORM_REPORTED_PERIOD ;
            sheet.Cells["E3"].Style.Font.Bold = true;
            //Комментарий
            sheet.Cells["F3"].Value = FORM_COMMENT;
            sheet.Cells["F3"].Style.Font.Bold = true;
            //ОКУД
            sheet.Cells["G3"].Value = FORM_OKUD;
            sheet.Cells["G3"].Style.Font.Bold = true;
            
            for (int i = 0; i < forms.Count(); i++)
            {
                sheet.Cells[i + COUNT_SKIP_ROW, 1].Value = forms[i].Index;
                sheet.Cells[i + COUNT_SKIP_ROW, 2].Value = forms[i].Name;
                sheet.Cells[i + COUNT_SKIP_ROW, 3].Value = forms[i].FormPeriod;
                sheet.Cells[i + COUNT_SKIP_ROW, 4].Value = forms[i].EndTime;
                sheet.Cells[i + COUNT_SKIP_ROW, 5].Value = forms[i].ReportedPeriod;
                sheet.Cells[i + COUNT_SKIP_ROW, 6].Value = forms[i].Comment;
                sheet.Cells[i + COUNT_SKIP_ROW, 7].Value = forms[i].Okud;
            }

            return await package.GetAsByteArrayAsync(ct);
        }
    }

    public async Task<byte[]> GetFileInfoOrg(List<InfoOrganization> infoOrg, CancellationToken ct)
    {
        using (var package = new ExcelPackage())
        {
            var sheet = package.Workbook.Worksheets.Add("Sheet1");
            
            //перенос текста
            sheet.Cells[4, 1, infoOrg.Count() + 4, 11].Style.WrapText = true;
            //горизонтальное выравнивание
            sheet.Cells[1,1, infoOrg.Count() + 4, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //вертикальное выравнивание
            sheet.Cells[1,1, infoOrg.Count() + 4, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            
            //объединяем ячейки
            //название
            sheet.Cells["A1:K1"].Merge = true;
            sheet.Cells["A1:K1"].Value = infoOrg.First().Name;
            sheet.Cells["A1:K1"].Style.Font.Bold = true;
            //ОКПО
            sheet.Cells["A2:K2"].Merge = true;
            sheet.Cells["A2:K2"].Value = $"ОКПО {infoOrg.First().Okpo}";
            sheet.Cells["A2:K2"].Style.Font.Bold = true;
            sheet.Cells["A2:K2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //дата формирования
            sheet.Cells["A3:K3"].Merge = true;
            sheet.Cells["A3:K3"].Value = $"Дата формирования - {DateTime.Now.ToShortDateString()}";
            //шапка данных
            //окпо
            sheet.Cells["A4"].Value = OKPO;
            sheet.Cells["A4"].Style.Font.Bold = true;
            //огрн
            sheet.Cells["B4"].Value = OGRN;
            sheet.Cells["B4"].Style.Font.Bold = true;
            //дата регистрации
            sheet.Cells["C4"].Value = DATE_REG;
            sheet.Cells["C4"].Style.Font.Bold = true;
            //инн
            sheet.Cells["D4"].Value = INN;
            sheet.Cells["D4"].Style.Font.Bold = true;
            //окато факт
            sheet.Cells["E4"].Value = OKATO_FACT;
            sheet.Cells["E4"].Style.Font.Bold = true;
            //окато рег
            sheet.Cells["F4"].Value = OKATO_REG;
            sheet.Cells["F4"].Style.Font.Bold = true;
            //октмо факт
            sheet.Cells["G4"].Value = OKTMO_FACT;
            sheet.Cells["G4"].Style.Font.Bold = true;
            //октмо рег
            sheet.Cells["H4"].Value = OKTMO_REG;
            sheet.Cells["H4"].Style.Font.Bold = true;
            //окогу
            sheet.Cells["I4"].Value = OKOGU;
            sheet.Cells["I4"].Style.Font.Bold = true;
            //окфс
            sheet.Cells["J4"].Value = OKFS;
            sheet.Cells["J4"].Style.Font.Bold = true;
            //окопф
            sheet.Cells["K4"].Value = OKOPF;
            sheet.Cells["K4"].Style.Font.Bold = true;

            for (int i = 0; i < infoOrg.Count(); i++)
            {
                sheet.Cells[i + 5, 1].Value = infoOrg[i].Okpo;
                sheet.Cells[i + 5, 2].Value = infoOrg[i].Ogrn;
                sheet.Cells[i + 5, 3].Value = infoOrg[i].DateReg;
                sheet.Cells[i + 5, 4].Value = infoOrg[i].Inn;
                sheet.Cells[i + 5, 5].Value = $"{infoOrg[i].OkatoFact.Code}-{infoOrg[i].OkatoFact.Name}";
                sheet.Cells[i + 5, 6].Value = $"{infoOrg[i].OkatoReg.Code}-{infoOrg[i].OkatoReg.Name}";
                sheet.Cells[i + 5, 7].Value = $"{infoOrg[i].OktmoFact.Code}-{infoOrg[i].OktmoFact.Name}";
                sheet.Cells[i + 5, 8].Value = $"{infoOrg[i].OktmoReg.Code}-{infoOrg[i].OktmoReg.Name}";
                sheet.Cells[i + 5, 9].Value = $"{infoOrg[i].Okogu.Code}-{infoOrg[i].Okogu.Name}";
                sheet.Cells[i + 5, 10].Value = $"{infoOrg[i].Okfs.Code}-{infoOrg[i].Okfs.Name}";
                sheet.Cells[i + 5, 11].Value = $"{infoOrg[i].Okopf.Code}-{infoOrg[i].Okopf.Name}";
            }

            return await package.GetAsByteArrayAsync(ct);
        }
    }
}