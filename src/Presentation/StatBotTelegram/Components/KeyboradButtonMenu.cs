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
            new KeyboardButton(NameButton.STAT_REPORT_IN_ELECTRON)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //Получение данных о кодах статистики и перечня форм
            new KeyboardButton(NameButton.GET_INFO_CODES_AND_LIST_FORMS),
            //Поиск специалиста, ответственного за форму
            new KeyboardButton(NameButton.SEARCH_EMPLOYEES)
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
            new KeyboardButton(NameButton.BY_OKUD),
            //По ФИО
            new KeyboardButton(NameButton.BY_FIO)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //По номеру телефона специалиста
            new KeyboardButton(NameButton.BY_PHONE_EMPLOYEE)
        },
        //третья строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.BACK)
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
            new KeyboardButton(NameButton.GET_INFO_ORGANIZATION),
            //Получить перечень форм
            new KeyboardButton(NameButton.GET_LIST_FORMS)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.BACK)
        }
    };
    
    private static KeyboardButton[][] _buttonsSearchOkpoInnOgrn = new KeyboardButton[][]
    {
        //первая строка кнопок
        new KeyboardButton[]
        {
            //По ОКПО
            new KeyboardButton(NameButton.BY_OKPO),
            //По ИНН
            new KeyboardButton(NameButton.BY_INN)
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //По ОГРН/ОГРНИП
            new KeyboardButton(NameButton.BY_OGRN)
        },
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.BACK)
        }
    };

    public static KeyboardButton[][] ButtonsMainMenu => _buttonsMainMenu;
    public static KeyboardButton[][] ButtonsSearchEmployeesMenu => _buttonsSearchEmployeesMenu;
    public static KeyboardButton[][] ButtonsInfoCodesAndListForm => _buttonsInfoCodesAndListForm;
    public static KeyboardButton[][] ButtonsSearchOkpoInnOgrn => _buttonsSearchOkpoInnOgrn;
}