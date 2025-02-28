namespace Application.Constants;

public static class TextMessage
{
    private static string _welcomeText = "Вас приветствует Telegram бот технической поддержки Свердловскстата (г. Курган)!" +
                                  "\n\nДля взаимодействия с чат-ботом используйте кнопки.";
    private static string _unknownCommand = "Команда не найдена, выберите команду из предложенного ниже меню!";
    private static string _searchEmployees = "Выберите критерий для поиска:";
    private static string _searchOkud = "Введите ОКУД формы:";
    private static string _selectCommand = "Из предложенного ниже меню выберите, что Вам требуется!";
    private static string _searchOkpo = "Введите ОКПО:";
    private static string _searchInn = "Введите ИНН:";
    private static string _searchOgrnOgrnip = "Введите ОГРН/ОГРНИП:";
    private static string _searchFioEmployee = "Введите фамилию специалиста:";
    private static string _searchPhoneEmployee = "Введите номер телефона специалиста:";
    
    public static string WelcomeText => _welcomeText; 
    public static string UnknownCommand => _unknownCommand; 
    public static string SearchEmployees => _searchEmployees; 
    public static string SearchOkud =>_searchOkud;
    public static string SelectCommand => _selectCommand; 
    public static string SearchOkpo => _searchOkpo; 
    public static string SearchInn => _searchInn; 
    public static string SearchOgrnOgrnip => _searchOgrnOgrnip; 
    public static string SearchFioEmployee => _searchFioEmployee; 
    public static string SearchPhoneEmployee => _searchPhoneEmployee; 
}