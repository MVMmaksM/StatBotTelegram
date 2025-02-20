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
            new KeyboardButton("Статотчетность в электронном виде")
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            new KeyboardButton("Получение данных о кодах статистики и перечня форм"),
            new KeyboardButton("Поиск специалиста, ответственного за форму")
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
            new KeyboardButton("По ОКУД формы"),
            new KeyboardButton("По ФИО")
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            new KeyboardButton("По номеру телефона специалиста")
        },
        //третья строка кнопок
        new KeyboardButton[]
        {
            new KeyboardButton("Назад")
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
            new KeyboardButton("Получить данные о кодах статистики организации"),
            new KeyboardButton("Получить перечень форм")
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            new KeyboardButton("Назад")
        }
    };
    
    private static KeyboardButton[][] _buttonsGetInfoOrganization = new KeyboardButton[][]
    {
        //первая строка кнопок
        new KeyboardButton[]
        {
            new KeyboardButton("По ОКПО"),
            new KeyboardButton("По ИНН")
        },
        //вторая строка кнопок
        new KeyboardButton[]
        {
            new KeyboardButton("По ОГРН/ОГРНИП")
        },
        new KeyboardButton[]
        {
            new KeyboardButton("Назад")
        }
    };

    public static KeyboardButton[][] ButtonsMainMenu => _buttonsMainMenu;
    public static KeyboardButton[][] ButtonsSearchEmployeesMenu => _buttonsSearchEmployeesMenu;
    public static KeyboardButton[][] ButtonsInfoCodesAndListForm => _buttonsInfoCodesAndListForm;
    public static KeyboardButton[][] ButtonsGetInfoOrganization => _buttonsGetInfoOrganization;
}