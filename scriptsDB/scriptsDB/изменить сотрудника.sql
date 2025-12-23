CREATE OR REPLACE PROCEDURE shem.update_employee(
    p_id_employees INTEGER,
    p_first_name VARCHAR(100) DEFAULT NULL,
    p_last_name VARCHAR(100) DEFAULT NULL,
    p_patronymic VARCHAR(100) DEFAULT NULL,
    p_phone VARCHAR(20) DEFAULT NULL,
    p_role_name VARCHAR(50) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_role_id INTEGER;
BEGIN
    
    IF p_phone IS NOT NULL AND NOT (p_phone ~ '^(\+7|8)[0-9]{10}$') THEN
        RAISE EXCEPTION 'Неверный формат телефона. Используйте: +7xxxxxxxxxx или 8xxxxxxxxxx';
    END IF;

	IF p_role_name IS NOT NULL THEN
        SELECT id_employee_roles INTO v_role_id
        FROM shem.employee_roles 
        WHERE name = p_role_name;
    END IF;
    
    UPDATE shem.employees 
    SET 
        first_name = COALESCE(p_first_name, first_name),
        last_name = COALESCE(p_last_name, last_name),
        patronymic = COALESCE(p_patronymic, patronymic),
        phone = COALESCE(p_phone, phone),
        id_employee_roles = COALESCE(v_role_id, id_employee_roles)
    WHERE id_employees = p_id_employees;
END;
$$;