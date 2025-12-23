CREATE OR REPLACE PROCEDURE shem.insert_employee(
    p_first_name VARCHAR(100),
    p_last_name VARCHAR(100),
    p_patronymic VARCHAR(100) DEFAULT NULL,
    p_phone VARCHAR(20) DEFAULT NULL,
    p_role_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NOT (p_phone ~ '^(\+7|8)[0-9]{10}$') THEN
        RAISE EXCEPTION 'Неверный формат телефона. Используйте: +7xxxxxxxxxx или 8xxxxxxxxxx';
    END IF;
    
    INSERT INTO shem.employees (
        first_name, 
        last_name, 
        patronymic, 
        phone, 
        id_employee_roles
    ) VALUES (
        p_first_name,
        p_last_name,
        p_patronymic,
        p_phone,
        p_role_id
    );
END;
$$;