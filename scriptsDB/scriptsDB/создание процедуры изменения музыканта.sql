CREATE OR REPLACE PROCEDURE shem.update_musician(
    p_id_musicians INTEGER,
    p_first_name VARCHAR(100) DEFAULT NULL,
    p_last_name VARCHAR(100) DEFAULT NULL,
    p_patronymic VARCHAR(100) DEFAULT NULL,
    p_birth_date DATE DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN

    IF NOT EXISTS (SELECT 1 FROM shem.musicians WHERE id_musicians = p_id_musicians) THEN
        RAISE EXCEPTION 'Музыкант % не найден', p_id_musicians;
    END IF;
        
    IF p_birth_date < '1300-01-01' THEN
            RAISE EXCEPTION 'Некорректная дата рождения';
    END IF;
    
    UPDATE shem.musicians 
    SET 
        first_name = COALESCE(p_first_name, first_name),
        last_name = COALESCE(p_last_name, last_name),
        patronymic = COALESCE(p_patronymic, patronymic),
        birth_date = COALESCE(p_birth_date, birth_date)
    WHERE id_musicians = p_id_musicians;
    
END;
$$;