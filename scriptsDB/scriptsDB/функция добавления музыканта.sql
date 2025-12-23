CREATE OR REPLACE FUNCTION shem.insert_musician(
    p_first_name VARCHAR(100),
    p_last_name VARCHAR(100),
    p_patronymic VARCHAR(100) DEFAULT NULL,
    p_birth_date DATE DEFAULT NULL
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    v_musician_id INTEGER;
BEGIN
    
    IF p_birth_date < '1300-01-01' THEN
        RAISE EXCEPTION 'Некорректная дата рождения';
    END IF;
    
    INSERT INTO shem.musicians (
        first_name, 
        last_name, 
        patronymic, 
        birth_date
    ) VALUES (
        p_first_name,
        p_last_name,
        p_patronymic,
        p_birth_date
    )
	RETURNING id_musicians INTO v_musician_id;
    
    RETURN v_musician_id;
END;
$$;