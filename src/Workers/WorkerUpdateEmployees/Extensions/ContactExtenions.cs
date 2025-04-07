using Domain.Entities;
using WorkerUpdateEmployees.Model;

namespace WorkerUpdateEmployees.Extensions;

public static class ContactExtenions
{
    public static List<Form> GetForms(this List<ContactDto> contacts)
    {
        var periods = new List<PeriodicityForm>();
        var departments = new List<Department>();
        var employees = new List<Employee>();
        var forms = new List<Form>();
        
        Department? department = null;

        for (int i = 0; i < contacts.Count; i++)
        {
            Console.WriteLine(i);
            
            Form form = new Form();
            var tmpEmployees = new List<Employee>();
            
            //добавляем ОКУД к форме
            form.Okud = int.TryParse(contacts[i].Okud, out var okud) ? okud : throw new Exception("Ошибка при конверте ОКУДа");
            //добавляем название формы
            form.Name = contacts[i].FormIndex;
            
            //ищем период в списке
            var period = periods
                .Find(p => p.Name.Trim() == contacts[i].Period.Trim());

            //если периода нет
            if (period is null)
            {
                //создаем
                period = new PeriodicityForm
                {
                    Name = contacts[i].Period
                };
                
                //добавляем в список
                periods.Add(period);
            }
            
            //добавляем период к форме
            form.PeriodicityForm = period;
            
            //сплитим для нахождения отдела
            var departmentsWithEmployees = contacts[i].KurganTel
                .Trim()
                .Split("-")
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToList();
            
            //если у формы нет сотрудника
            //то для сотрудника будет указан 0
            //то пропускаем такую формк
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
                    
                    //ищем отдел в списке
                    department = departments
                        .Find(d => d.Name == nameDepartment);
                    
                    //если отдел не найден
                    //то добавляем его в список
                    if (department is null)
                    {
                        department = new Department
                        {
                            Name = nameDepartment
                        };
                        
                        departments.Add(department);
                    }

                    //сплитим и получаем фио и номер телефона
                    //сотрудника
                    var fioWithPhone = dpe
                        .Split(upperChar)[1]
                        .Trim();
                    //получаем сотрудника
                    var employee = GetEmployee(fioWithPhone);

                    //ищем сотрудника в списке
                    var searchEmployee = employees.Find(e =>
                        e.FirstName == employee.FirstName &&
                        e.LastName == employee.LastName &&
                        e.SurName == employee.SurName);

                    //если сотрудник не найден
                    //то добавляем его с список
                    if (searchEmployee is null)
                    {
                        employees.Add(employee);
                        tmpEmployees.Add(employee);
                    }
                    else
                    {
                        //если сотрудник найден и у него не заполнен
                        //отдел, то заполняем
                       if(searchEmployee.Department is null)
                           searchEmployee.Department = department;
                       
                       tmpEmployees.Add(searchEmployee);
                    }
                }
            }
            else
            {
                //получаем сотрудника
                var employee = GetEmployee(contacts[i].KurganTel.Trim());
                
                //ищем сотрудника в списке
                var searchEmployee = employees.Find(e =>
                    e.FirstName == employee.FirstName &&
                    e.LastName == employee.LastName &&
                    e.SurName == employee.SurName);
                
                //если сотрудник не найден
                //то добавляем его с список
                if (searchEmployee is null)
                {
                    employees.Add(employee);
                    tmpEmployees.Add(employee);
                }
                else
                {
                    //если найден
                    tmpEmployees.Add(searchEmployee);
                }
            }
            
            form.Employees = tmpEmployees;
            forms.Add(form);
        }
        
        return forms;
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