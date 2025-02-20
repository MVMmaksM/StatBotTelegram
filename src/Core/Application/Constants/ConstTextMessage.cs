namespace Application.Constants;

public static class ConstTextMessage
{
    private static string _welcomeText = "Вас приветствует Telegram бот технической поддержки Свердловскстата (г. Курган)!" +
                                  "\n\nДля взаимодействия с чат-ботом используйте кнопки.";
    private static string _unknownCommand = "Команда не найдена, выберите команду из предложенного ниже меню";
    private static string _searchEmployees = "Выберите критерий для поиска:";
    private static string _searchOkud = "Введите ОКУД формы:";
    private static string _selectCommand = "Из предложенного ниже меню выберите, что Вам требуется!";
    
    public static string WelcomeText => _welcomeText; 
    public static string UnknownCommand => _unknownCommand; 
    public static string SearchEmployees => _searchEmployees; 
    public static string SearchOkud =>_searchOkud;
    public static string SelectCommand => _selectCommand; 
}