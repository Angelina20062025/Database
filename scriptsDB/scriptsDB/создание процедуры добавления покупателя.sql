CREATE OR REPLACE PROCEDURE shem.add_customer(
    p_first_name VARCHAR(100),
    p_last_name VARCHAR(100),
    p_patronymic VARCHAR(100) DEFAULT NULL,
    p_phone VARCHAR(20) DEFAULT NULL,
    p_email VARCHAR(100) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_customer_id INTEGER;
BEGIN

	IF p_phone IS NOT NULL AND p_phone != '' THEN
    	IF NOT (p_phone ~ '^(\+7|8)[0-9]{10}$') THEN
        	RAISE EXCEPTION 'Неверный формат телефона. Используйте: +7xxxxxxxxxx или 8xxxxxxxxxx';
    	END IF;
	END IF;
    
    IF p_email IS NOT NULL AND p_email != '' THEN
        IF NOT (p_email ~ '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$') THEN
            RAISE EXCEPTION 'Неверный формат email';
        END IF;
    
    IF EXISTS (SELECT 1 FROM shem.customers WHERE phone = p_phone) THEN
        RAISE EXCEPTION 'Покупатель с телефоном % уже существует', p_phone;
    END IF;

	IF EXISTS (SELECT 1 FROM shem.customers WHERE email = p_email) THEN
            RAISE EXCEPTION 'Покупатель с email % уже существует', p_email;
        END IF;
    END IF;
    
    INSERT INTO shem.customers (
        first_name, 
        last_name, 
        patronymic, 
        phone, 
        email
    ) VALUES (
        p_first_name,
        p_last_name,
        NULLIF(p_patronymic, ''),
		NULLIF(p_phone, ''),
        NULLIF(p_email, '')
    );
END;
$$;