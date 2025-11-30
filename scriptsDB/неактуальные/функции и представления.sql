-- Представление для покупателей с полными именами
CREATE OR REPLACE VIEW shem.customers_view AS
SELECT 
    id_customers,
    first_name,
    last_name,
    patronymic,
    shem.get_full_name(first_name, last_name, patronymic) as full_name,
    phone,
    email
FROM shem.customers;

-- Представление для музыкантов с полными именами
CREATE OR REPLACE VIEW shem.musicians_view AS
SELECT 
    id_musicians,
    first_name,
    last_name,
    patronymic,
    shem.get_full_name(first_name, last_name, patronymic) as full_name,
    birth_date,
    mr.name as role_name,
    bio
FROM shem.musicians m
LEFT JOIN shem.musician_roles mr ON m.id_musician_roles = mr.id_musician_roles;

-- Представление для удобного просмотра сотрудников с полными именами
CREATE OR REPLACE VIEW shem.employees_view AS
SELECT 
    e.id_employees,
    e.first_name,
    e.last_name,
    e.patronymic,
    shem.get_full_name(e.first_name, e.last_name, e.patronymic) as full_name,
    e.phone,
    er.name as role_name,
    u.id_users,
    u.login,
    CASE 
        WHEN u.id_users IS NOT NULL THEN 'Да'
        ELSE 'Нет'
    END as has_account
FROM shem.employees e
LEFT JOIN shem.employee_roles er ON e.id_employee_roles = er.id_employee_roles
LEFT JOIN shem.users u ON e.id_employees = u.id_employees;

-- Функция для получения полного имени
CREATE OR REPLACE FUNCTION shem.get_full_name(
    first_name VARCHAR, 
    last_name VARCHAR, 
    patronymic VARCHAR DEFAULT NULL
)
RETURNS VARCHAR AS $$
BEGIN
    IF patronymic IS NOT NULL THEN
        RETURN first_name || ' ' || patronymic || ' ' || last_name;
    ELSE
        RETURN first_name || ' ' || last_name;
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Функция для пересчета статистики продаж
CREATE OR REPLACE FUNCTION shem.recalculate_sales_stats()
RETURNS void AS $$
BEGIN
    -- Обнуляем и пересчитываем текущий год
    UPDATE shem.record 
    SET current_year_sales = (
        SELECT COALESCE(SUM(quantity), 0)
        FROM shem.record_sales 
        WHERE id_record = shem.record.id_record 
        AND EXTRACT(YEAR FROM sale_date) = EXTRACT(YEAR FROM CURRENT_DATE)
    );
    
    -- Обнуляем и пересчитываем прошлый год
    UPDATE shem.record 
    SET last_year_sales = (
        SELECT COALESCE(SUM(quantity), 0)
        FROM shem.record_sales 
        WHERE id_record = shem.record.id_record 
        AND EXTRACT(YEAR FROM sale_date) = EXTRACT(YEAR FROM CURRENT_DATE) - 1
    );
END;
$$ LANGUAGE plpgsql;