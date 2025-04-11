using WorkerUpdateEmployees.Model;

namespace WorkerUpdateEmployees.Extensions;

public static class ContactExtenions
{
    public static List<Contact> GetContacts(this List<ContactDto> contactsDto)
    {
        var contacts = new List<Contact>();

        for (int i = 0; i < contactsDto.Count; i++)
        {
            var contact = new Contact();
            var employees = new List<Employee>();
            
            //добавляем ОКУД к форме
            contact.Okud = int.TryParse(contactsDto[i].Okud, out var okud) ? okud : throw new Exception("Ошибка при конверте ОКУДа");
            //добавляем название формы
            contact.FormIndex = contactsDto[i].FormIndex;
            //добавляем период
            contact.Period = contactsDto[i].Period;
            //сплитим для нахождения отдела
            var departmentsWithEmployees = contactsDto[i].KurganTel
                .Trim()
                .Split("-")
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToList();
            
            //если у формы нет сотрудника
            //то для сотрудника будет указан 0
            //то пропускаем такую форму
            if(departmentsWithEmployees.Count == 1 && departmentsWithEmployees[0] == "0")
                continue;
            
            //если есть отделы
            //то начинаем с ними работать
            if (departmentsWithEmployees.Count > 1)
            {
                foreach (var dpe in departmentsWithEmployees)
                {
                    //находим в строке 1 заглавную букву
                    var upperChar = dpe.FirstOrDefault(c => char.IsUpper(c));
                    
                    //сплитим по заглавной букве
                    //и получаем название отдела
                    var nameDepartment = dpe
                        .Split(upperChar)[0]
                        .Trim();
                    
                    //сплитим и получаем фио и номер телефона
                    //сотрудника
                    var fioWithPhone = dpe
                        .Replace(nameDepartment, string.Empty)
                        .Trim();
                    //получаем сотрудника
                    var employee = GetEmployee(fioWithPhone);
                    //добавляем название департамента
                    employee.Department = nameDepartment;
                    //добавляем сотрудника в список
                    employees.Add(employee);
                }
            }
            else
            {
                //получаем сотрудника
                var employee = GetEmployee(contactsDto[i].KurganTel.Trim());
                //добавляем сотрудника в список
                employees.Add(employee);
            }
            
            contact.Employees = employees.ToArray();
            contacts.Add(contact);
        }
        
        return contacts;
    }
    private static Employee GetEmployee(string fioWithPhone)
    {
        var employee = new Employee();
        var splitter = fioWithPhone
            .Trim()
            .Split(" ");
        
        //фамилия
        employee.LastName = splitter[0];
        //имя
        employee.FirstName = splitter[1];
        //отчество
        employee.SurName = splitter[2];

        var phone = string.Empty;

        for (int i = 3; i < splitter.Length; i++)
        {
            phone += splitter[i];
        }
        //номер телефона
        //удаляем скобки
        phone = phone
            .Replace("(", "")
            .Replace(")", "");
        
        //если начинается на 8
        //то убираем ее
        if(phone.StartsWith("8"))
            phone = phone.Substring(1);
        
        employee.Phone = phone;
        
        return employee;
    }
}