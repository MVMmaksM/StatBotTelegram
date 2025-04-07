create type employee AS (
    firstname varchar(128),
    lastname varchar(128),
    surname varchar(128),
    phone varchar(128),
    department varchar(128)
    );

create type contact as (
    okud int,
    form_index varchar(32),
    period varchar(32),
    employees employee[]
    );

create or replace procedure update_contact(contacts jsonb)
language plpgsql
as $$
declare
    l_employees_id int[];
	l_period_id int;
	l_depertment_id int;
	l_form_id int;
	l_contact record;
	l_employee record;
	l_employee_id int;
	l_employees_json jsonb;
begin
	--очищаем таблицы
    delete from employee_forms;
    delete from employees;
    delete from forms;

    --проходимся по формам
    for l_contact in (select * from jsonb_populate_recordset(null::contact, contacts))
        loop
    
            --проверяем существование периодичности формы
            if not exists (select 1 from periodicity_forms pf where pf.name = l_contact.period) then
                insert into periodicity_forms (name)
                values (l_contact.period);
                select currval('periodicity_forms_id_seq') into l_period_id;
            else
                select id into l_period_id  from periodicity_forms pf where pf.name = l_contact.period;
            end if;
            
            --проходим по списку сотрудников
            l_employees_json := to_jsonb(l_contact.employees);
            for l_employee in (SELECT * from jsonb_populate_recordset(null::employee, l_employees_json))             
                    loop
                        --проверяем существование отдела			
                        if not exists (select 1 from departments d where d.name = l_employee.department) then
                            insert into departments(name)
                            values(l_employee.department);
                            select currval('departments_id_seq') into l_depertment_id;
                        else
                            select id into l_depertment_id from departments d where d.name = l_employee.department;
                        end if;
                        
                        --проверяем существование сотрудника
                        if not exists (
                                        select 1 
                                        from employees e 
                                        where e.firstname = l_employee.firstname 
                                            and e.lastname = l_employee.lastname 
                                            and e.surname = l_employee.surname) then
                            insert into employees(firstname, lastname, surname, phone, department_id)		
                            values(l_employee.firstname, l_employee.lastname, l_employee.surname, l_employee.phone, l_depertment_id);				
                            l_employees_id = array_append(l_employees_id, currval('employees_id_seq'));
                        else
                            l_employees_id = array_append(l_employees_id, (select id 
                                                                          from employees e 
                                                                           where e.firstname = l_employee.firstname 
                                                                             and e.lastname = l_employee.lastname 
                                                                             and e.surname = l_employee.surname));
                        end if;
                    end loop;            
                    --добавляем форму
            insert into forms(name, okud, periodicity_form_id)
            values (l_contact.form_index, l_contact.okud, l_period_id);
            select currval('forms_id_seq') into l_form_id;
            
            --привязываем сотрудников к форме
            FOREACH l_employee_id IN ARRAY l_employees_id
            loop
                        insert into employee_forms(employee_id, form_id)
                        values(l_employee_id, l_form_id);
            end loop;                    
        end loop;
end;$$;