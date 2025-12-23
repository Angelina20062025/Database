CREATE OR REPLACE PROCEDURE shem.update_customer(
    p_id_customers INTEGER,
    p_first_name VARCHAR(100) DEFAULT NULL,
    p_last_name VARCHAR(100) DEFAULT NULL,
    p_patronymic VARCHAR(100) DEFAULT NULL,
    p_phone VARCHAR(20) DEFAULT NULL,
    p_email VARCHAR(100) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM shem.customers WHERE id_customers = p_id_customers) THEN
        RAISE EXCEPTION 'Покупатель с ID % не найден', p_id_customers;
    END IF;
    
    IF p_phone IS NOT NULL AND p_phone != '' AND
		NOT (p_phone ~ '^(\+7|8)[0-9]{10}$') THEN
        RAISE EXCEPTION 'Неверный формат телефона. Используйте: +7xxxxxxxxxx или 8xxxxxxxxxx';
    END IF;
    
    IF p_email IS NOT NULL AND p_email != '' AND 
       NOT (p_email ~ '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$') THEN
        RAISE EXCEPTION 'Неверный формат email';
    END IF;
    
    UPDATE shem.customers 
    SET 
        first_name = COALESCE(p_first_name, first_name),
        last_name = COALESCE(p_last_name, last_name),
        patronymic = COALESCE(p_patronymic, patronymic),
        phone = COALESCE(p_phone, phone),
        email = COALESCE(p_email, email)
    WHERE id_customers = p_id_customers;
END;
$$;