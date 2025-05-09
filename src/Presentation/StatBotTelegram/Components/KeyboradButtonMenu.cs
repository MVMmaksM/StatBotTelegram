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
            //По индексу формы
            new KeyboardButton(NameButton.BY_INDEX_FORM),
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            //По номеру телефона специалиста
            new KeyboardButton(NameButton.BY_PHONE_EMPLOYEE),
            //По ФИО
            new KeyboardButton(NameButton.BY_FIO)
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

    private static KeyboardButton[][] _buttonsGetListForm = new KeyboardButton[][]
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
        //третья строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.DOWNLOAD_TEMPLATE)
        },
        //четвертая строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.BACK)
        }
    };
    
    private static KeyboardButton[][] _buttonsGetInfoOrg = new KeyboardButton[][]
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
        //третья строка кнопок
        new KeyboardButton[]
        {
            //Назад
            new KeyboardButton(NameButton.BACK)
        }
    };

    public static KeyboardButton[][] ButtonsMainMenu => _buttonsMainMenu;
    public static KeyboardButton[][] ButtonsSearchEmployeesMenu => _buttonsSearchEmployeesMenu;
    public static KeyboardButton[][] ButtonsInfoCodesAndListForm => _buttonsInfoCodesAndListForm;
    public static KeyboardButton[][] ButtonsGetListForm => _buttonsGetListForm;
    public static KeyboardButton[][] ButtonsGetInfoOrg => _buttonsGetInfoOrg;
}