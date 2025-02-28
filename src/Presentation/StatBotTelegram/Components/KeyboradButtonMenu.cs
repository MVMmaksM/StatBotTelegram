using Application.Constants;
using Telegram.Bot.Types.ReplyMarkups;
namespace StatBotTelegram.Components;

public static class KeyboradButtonMenu 
{
    /// <summary>
    /// кнопки главного меню
    /// </summary>
    private static KeyboardButton[][] _buttonsMainMenu = new KeyboardButton[][]
    {
        //первая строка кнопок
        new KeyboardButton[]
        {
            //Статотчетность в электронном виде
            new KeyboardButton(NameButton.StatReportInElectron)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //Получение данных о кодах статистики и перечня форм
            new KeyboardButton(NameButton.GetInfoCodesAndListForms),
            //Поиск специалиста, ответственного за форму
            new KeyboardButton(NameButton.SearchEmployees)
        }
    };
    
    /// <summary>
    /// кнопки меню поиска сотрудника
    /// </summary>
    private static KeyboardButton[][] _buttonsSearchEmployeesMenu = new KeyboardButton[][]
    {
        //первая строка кнопок
        new KeyboardButton[]
        {
            //По ОКУД формы
            new KeyboardButton(NameButton.ByOkud),
            //По ФИО
            new KeyboardButton(NameButton.ByFio)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //По номеру телефона специалиста
            new KeyboardButton(NameButton.ByPhoneEmployee)
        },
        //третья строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.Back)
        }
    };
    
    /// <summary>
    /// кнопки получения данных о кодах статистики и перечня форм
    /// </summary>
    private static KeyboardButton[][] _buttonsInfoCodesAndListForm = new KeyboardButton[][]
    {
        //первая строка кнопок
        new KeyboardButton[]
        {
            //Получить данные о кодах статистики организации
            new KeyboardButton(NameButton.GetInfoOrganization),
            //Получить перечень форм
            new KeyboardButton(NameButton.GetListForms)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.Back)
        }
    };
    
    private static KeyboardButton[][] _buttonsSearchOkpoInnOgrn = new KeyboardButton[][]
    {
        //первая строка кнопок
        new KeyboardButton[]
        {
            //По ОКПО
            new KeyboardButton(NameButton.ByOkpo),
            //По ИНН
            new KeyboardButton(NameButton.ByInn)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //По ОГРН/ОГРНИП
            new KeyboardButton(NameButton.ByOgrn)
        },
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.Back)
        }
    };

    public static KeyboardButton[][] ButtonsMainMenu => _buttonsMainMenu;
    public static KeyboardButton[][] ButtonsSearchEmployeesMenu => _buttonsSearchEmployeesMenu;
    public static KeyboardButton[][] ButtonsInfoCodesAndListForm => _buttonsInfoCodesAndListForm;
    public static KeyboardButton[][] ButtonsSearchOkpoInnOgrn => _buttonsSearchOkpoInnOgrn;
}