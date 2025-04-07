using System.Text.RegularExpressions;
using Domain.Entities;
using WorkerUpdateEmployees.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using WorkerUpdateEmployees.Model;

namespace WorkerUpdateEmployees.Services;

public class Parser : IParser
{
    public List<ContactDto> ParseContact(string content)
    {
        //определян=ем начало списка
        var indexStart = content.IndexOf('[');
        
        //удаляем ненужные символы
        //и оборачиваем в кавычки поля
        var json = content
            .Substring(indexStart)
            .Replace("okud", "\"okud\"")
            .Replace("form_index", "\"form_index\"")
            .Replace("period", "\"period\"")
            .Replace("ekat_tel", "\"ekat_tel\"")
            .Replace("kurgan_tel", "\"kurgan_tel\"")
            .Replace(";", "");
        
        //удаляея дефисы в номерах телефона
        json = Regex.Replace(json, @"(?<=\d)-(?=\d)", "");
        
        return JsonConvert.DeserializeObject<List<ContactDto>>(json);
    }
    
    //deprecated
    /*
    public void ParseFormEmployee(string content)
    {
        var contentReplaced = content.Replace("&nbsp;", " ").Replace("<br>", " ").Replace("</br>", " ").Replace("\r\n", " ");
        var formEmployee = new List<Form>();
        var departments = new List<Department>();
        var employees = new List<Employee>();
        
        var html = new HtmlDocument();
        html.LoadHtml(contentReplaced);
        var document = html.DocumentNode;

        //вытаскиваем body таблицы
        var tbody = document.QuerySelector("tbody");
        //получаем tr таблицы
        var rows = tbody.ChildNodes;

        for (int i = 3; i < rows.Count; i = i + 2)
        {
            var tmpEmployees = new List<Employee>();
            
            //получаем колонки td строки
            var columns = rows[i].ChildNodes;
            //берем первую колонку, в которой
            //ОКУД, название формы и периодичность
            var colForms = columns[1].ChildNodes;
            //берем 1 строку, в которой должен быть ОКУД
            var okudStr = colForms[0].InnerText.Trim();
            
            var form = new Form();
            var periodicityForm = new PeriodicityForm();

            //сплиттим строку с ОКУДом
            //потому что в ней могут быть
            //название и периодичность (ДАФЛ)
            var splitterOkud = okudStr.Split(" ");
            
            //если в строке с ОКУДом
            //3 записи
            //значит там еще название формы и периодичность
            if (splitterOkud.Length == 3 && char.IsDigit(splitterOkud[0], 1))
            {
                if (int.TryParse(splitterOkud[0], out var okud))
                {
                    form.Okud = okud;
                    form.Name = splitterOkud[1];
                    //периодичность формы
                    periodicityForm.Name = splitterOkud[2];
                }
                else
                {
                    throw new Exception("ОКУД не является числом!");
                }
            }
            else
            {
                if (int.TryParse(splitterOkud[0], out var okud))
                {
                    form.Okud = okud;
                }
                else
                {
                    throw new Exception("ОКУД не является числом!");
                }

                //название формы
               var nameForm= colForms[2].InnerText.Trim();
               if (string.IsNullOrWhiteSpace(nameForm))
                   throw new Exception($"Название формы не может быть пустым!\nНомер строки {i}");
            }

            //берем столбец с сотрудником
            var colEmployees = columns[5].ChildNodes;
            
            var employee = new Employee();
            
            //выбираем только не пустые
            var withoutEmpty = colEmployees
                .Where(e => !string.IsNullOrWhiteSpace(e.InnerHtml));

            //если получился один узел
            //значит ФИО и номер телефона в одном узле
            if (withoutEmpty.Count() == 1)
            {
                //если строка начинается на этот тэг
                //значит в ней отделы
                if (withoutEmpty.FirstOrDefault().InnerHtml.StartsWith(
                        "<span style=\"text-decoration: underline;\"><span style=\"text-decoration: underline;\">"))
                {
                    var splitterStr = withoutEmpty
                        .FirstOrDefault().InnerHtml
                        .Split("<span style=\"text-decoration: underline;\"><span style=\"text-decoration: underline;\">")
                        .Where(s => !string.IsNullOrWhiteSpace(s));

                    foreach (var splitStr in splitterStr)
                    {
                        var tmpEmployee = new Employee();
                        
                        var departmentName = splitStr
                            .Split(":")[0].Trim();
                        
                        var employeeStr = splitStr
                            .Split("<br>")[1].Trim();

                        var phone = splitStr
                            .Split("<br>")[2].Trim();

                        tmpEmployee.LastName = employeeStr.Split(" ")[0].Trim();
                        tmpEmployee.FirstName = employeeStr.Split(" ")[1].Trim();
                        tmpEmployee.SurName = employeeStr.Split(" ")[2].Trim();
                        tmpEmployee.Phone = phone;

                        var departmentsSearch = departments
                            .Find(d => d.Name == departmentName);
                        
                        //если отдела еще нет в списке
                        //то добавляем его
                        if (departmentsSearch is null)
                        {
                            var newDepartment = new Department
                            {
                                Name = departmentName
                            };
                            
                            departments.Add(newDepartment);
                            tmpEmployee.Department = newDepartment;
                        }
                        else
                        {
                            tmpEmployee.Department = departmentsSearch;
                        }
                        
                        //ищем сотрудника в списке
                        var searchEmployee = employees.Find(e => e.FirstName == tmpEmployee.FirstName
                                                                 && e.LastName == tmpEmployee.LastName
                                                                 && e.SurName == tmpEmployee.SurName);
                        //если не нашли то добавляем в список
                        if (searchEmployee is null)
                        {
                            employees.Add(tmpEmployee);
                            tmpEmployees.Add(tmpEmployee);
                        }
                        //если нашли, то смотрим заполнен ли отдел
                        else
                        {
                            if(searchEmployee.Department is null && tmpEmployee.Department is not null){}
                                searchEmployee.Department = tmpEmployee.Department;
                                
                            tmpEmployees.Add(searchEmployee);
                        }
                    }
                }
                else
                {
                    var employeeSplitted = withoutEmpty
                        .FirstOrDefault().InnerHtml
                        .Trim()
                        .Replace("<br>", " ")
                        .Split(" ");

                    if (employeeSplitted.Length == 5)
                    {
                        employee.LastName = employeeSplitted[0].Trim();
                        employee.FirstName = employeeSplitted[1].Trim();
                        employee.SurName = employeeSplitted[2].Trim();
                        employee.Phone = employeeSplitted[3] + employeeSplitted[4];
                        
                        //ищем сотрудника в списке
                        var searchEmployee = employees.Find(e => e.FirstName == employee.FirstName
                                                                 && e.LastName == employee.LastName
                                                                 && e.SurName == employee.SurName);
                        //если не нашли
                        if (searchEmployee is null)
                        {
                            employees.Add(employee);
                            tmpEmployees.Add(employee);
                        }
                        //если нашли
                        else
                        {
                            tmpEmployees.Add(employee);
                        }
                    }

                    if (employeeSplitted.Length == 4)
                    {
                        employee.LastName = employeeSplitted[0];
                        employee.FirstName = employeeSplitted[1];
                        employee.SurName = employeeSplitted[2];
                        employee.Phone = employeeSplitted[3];
                        
                        //ищем сотрудника в списке
                        var searchEmployee = employees.Find(e => e.FirstName == employee.FirstName
                                                                 && e.LastName == employee.LastName
                                                                 && e.SurName == employee.SurName);
                        //если не нашли
                        if (searchEmployee is null)
                        {
                            employees.Add(employee);
                            tmpEmployees.Add(employee);
                        }
                        //если нашли
                        else
                        {
                            tmpEmployees.Add(employee);
                        }
                    }
                }
            }
            
            //если получилось 2 узла
            //значит номер телефона и ФИО в разных узлах
            if (withoutEmpty.Count() == 2)
            {
                //выбираем номер телефона
                //если строка начинается с цифры
                //значит номер телефона
                var phone = withoutEmpty
                    .Where(e => char.IsDigit(e.InnerHtml.Trim(), 0))
                    .FirstOrDefault()
                    .InnerHtml;

                //выбираем ФИО
                var fioSplitted = withoutEmpty
                    .Where(e => !char.IsDigit(e.InnerHtml.Trim(), 0))
                    .FirstOrDefault().InnerHtml
                    .Trim()
                    .Split(" ");
                
                employee.LastName = fioSplitted[0];
                employee.FirstName = fioSplitted[1];
                employee.SurName = fioSplitted[2];
                employee.Phone = phone;
                
                //ищем сотрудника в списке
                var searchEmployee = employees.Find(e => e.FirstName == employee.FirstName
                                                         && e.LastName == employee.LastName
                                                         && e.SurName == employee.SurName);
                //если не нашли
                if (searchEmployee is null)
                {
                    employees.Add(employee);
                    tmpEmployees.Add(employee);
                }
                //если нашли
                else
                {
                    tmpEmployees.Add(employee);
                }
            }

            form.Id = i;
            form.Employees = tmpEmployees;
            formEmployee.Add(form);

            Console.WriteLine(i);
            Console.WriteLine($"Фамилия: {employee.LastName} имя:{employee.FirstName} отчество:{employee.SurName} телефон:{employee.Phone}");
        }
        
        formEmployee.ForEach(f =>
        {
            Console.WriteLine($"Id формы: {f.Id} окуд формы: {f.Okud}");
            Console.WriteLine("Сотрудники:");
            f.Employees.ForEach(e =>
            {
                Console.WriteLine($"Фамилия: {e.LastName} Имя: {e.FirstName} Отчество {e.SurName} Отдел: {e?.Department?.Name}");
            });
        });
        
        Console.WriteLine("End");
    }
    */
}