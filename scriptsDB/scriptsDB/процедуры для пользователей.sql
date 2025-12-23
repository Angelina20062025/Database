CREATE OR REPLACE PROCEDURE shem.insert_user(
    p_login VARCHAR(50),
    p_password VARCHAR(100),
    p_id_employees INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_password_hash TEXT;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM shem.employees WHERE id_employees = p_id_employees AND is_deleted = false) THEN
        RAISE EXCEPTION 'Сотрудник с ID % не найден или удален', p_id_employees;
    END IF;
    
    IF EXISTS (SELECT 1 FROM shem.users WHERE login = p_login AND is_deleted = false) THEN
        RAISE EXCEPTION 'Логин "%" уже существует', p_login;
    END IF;
    
    v_password_hash := crypt(p_password, gen_salt('bf'));
    
    INSERT INTO shem.users (login, password_hash, id_employees)
    VALUES (p_login, v_password_hash, p_id_employees);
END;
$$;

CREATE OR REPLACE PROCEDURE shem.update_user(
    p_id_users INTEGER,
    p_login VARCHAR(50) DEFAULT NULL,
    p_password VARCHAR(100) DEFAULT NULL,
    p_id_employees INTEGER DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM shem.users WHERE id_users = p_id_users AND is_deleted = false) THEN
        RAISE EXCEPTION 'Пользователь с ID % не найден', p_id_users;
    END IF;
    
    IF p_login IS NOT NULL THEN
        IF EXISTS (SELECT 1 FROM shem.users WHERE login = p_login AND id_users != p_id_users AND is_deleted = false) THEN
            RAISE EXCEPTION 'Логин "%" уже используется другим пользователем', p_login;
        END IF;
    END IF;
    
    IF p_id_employees IS NOT NULL THEN
        IF NOT EXISTS (SELECT 1 FROM shem.employees WHERE id_employees = p_id_employees AND is_deleted = false) THEN
            RAISE EXCEPTION 'Сотрудник с ID % не найден или удален', p_id_employees;
        END IF;
    END IF;
    
    UPDATE shem.users 
    SET 
        login = COALESCE(p_login, login),
        password_hash = CASE 
            WHEN p_password IS NOT NULL AND p_password != '' THEN 
                crypt(p_password, gen_salt('bf'))
            ELSE 
                password_hash
        END,
        id_employees = COALESCE(p_id_employees, id_employees)
    WHERE id_users = p_id_users;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.archive_user(
    p_id_users INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE shem.users 
    SET is_deleted = true
    WHERE id_users = p_id_users;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Пользователь с ID % не найден', p_id_users;
    END IF;
END;
$$;