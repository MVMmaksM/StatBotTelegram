do $$
begin
    drop type if exists contact;
    drop type if exists employee;

    create type employee AS (
        firstname varchar(128),
        lastname varchar(128),
        surname varchar(128),
        phone varchar(128),
        department varchar(128)
        );

    create type contact as (
        okud int,
        form_index varchar(64),
        period varchar(64),
        employees employee[]
        );
end$$;

CREATE OR REPLACE PROCEDURE public.update_contact(IN contacts jsonb)
LANGUAGE plpgsql
AS $procedure$
declare
    l_employees_id int[];
	l_period_id int;
	l_department_id int;
	l_form_id int;
	l_contact record;
	l_employee record;
	l_employee_id int;
	l_employees_json jsonb;
	l_exist_employee_id int;
begin
	--очищаем таблицы
    delete from employee_forms;
    delete from employees;
    delete from forms;
    
    --проходимся по формам
    for l_contact in (select * from jsonb_populate_recordset(null::contact, contacts))
    loop
        --проверяем существование периодичности формы
        select id into l_period_id  from periodicity_forms pf where lower(pf.name) = lower(btrim(l_contact.period));
        --если период не найден
        if l_period_id is null then
            insert into periodicity_forms (name)
            values (btrim(l_contact.period));
            select currval('periodicity_forms_id_seq') into l_period_id;
        end if;
            
        --проходим по списку сотрудников
        l_employees_json := to_jsonb(l_contact.employees);
        for l_employee in (SELECT * from jsonb_populate_recordset(null::employee, l_employees_json)) 		
        loop
            --если у сотрудника заполнен отдел
            if l_employee.department is not null and btrim(l_employee.department) != '' then	
                --проверяем существование отдела	
                select id into l_department_id from departments d where lower(d.name) = lower(btrim(l_employee.department));
                --если отдел не найден
                if l_department_id is null then
                    insert into departments(name)
                    values(btrim(l_employee.department));
                    select currval('departments_id_seq') into l_department_id;
                end if;
            end if;
                
            --проверяем существование сотрудника
            select id into l_exist_employee_id
            from employees e
            where lower(e.firstname) = lower(btrim(l_employee.firstname))
                and lower(e.lastname) = lower(btrim(l_employee.lastname))
                and lower(e.surname) = lower(btrim(l_employee.surname));
    
            --если не существует, то добавляем
            if l_exist_employee_id is null then
                insert into employees(firstname, lastname, surname, phone, department_id)		
                values(	btrim(l_employee.firstname), 
                        btrim(l_employee.lastname), 
                        btrim(l_employee.surname), 
                        btrim(l_employee.phone), 
                        l_department_id);				
                l_employees_id = array_append(l_employees_id, currval('employees_id_seq'));
            else
                --если департамен у сотрудника не заполнен и департамент пришел,
                --то добавляем его сотруднику
                if (select department_id from employees where id = l_exist_employee_id) is null	
                            and l_department_id	is not null then
                    update employees
                    set department_id = l_department_id
                    where id = l_exist_employee_id;
                end if;
                        l_employees_id = array_append(l_employees_id, l_exist_employee_id);
            end if;
        end loop;
            
        --добавляем форму
        insert into forms(name, okud, periodicity_form_id)
        values (btrim(l_contact.form_index), l_contact.okud, l_period_id);
        select currval('forms_id_seq') into l_form_id;
    
        --привязываем сотрудников к форме
        FOREACH l_employee_id IN ARRAY l_employees_id
        loop
            insert into employee_forms(employee_id, form_id)
            values(l_employee_id, l_form_id);
        end loop; 
        l_employees_id = ARRAY[]::int[];
    end loop;
end;$procedure$;

