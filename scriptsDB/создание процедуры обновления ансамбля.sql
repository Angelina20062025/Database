CREATE OR REPLACE PROCEDURE shem.update_ensemble_info(
    p_id_ensembles INTEGER,
    p_name VARCHAR(100) DEFAULT NULL,
    p_ensemble_type_name VARCHAR(50) DEFAULT NULL,
    p_founded_date DATE DEFAULT NULL,
    p_description TEXT DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_ensemble_type_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM shem.ensembles WHERE id_ensembles = p_id_ensembles) THEN
        RAISE EXCEPTION 'Ансамбль с ID % не найден', p_id_ensembles;
    END IF;
    
    IF p_ensemble_type_name IS NOT NULL THEN
        SELECT id_ensemble_types INTO v_ensemble_type_id
        FROM shem.ensemble_types
        WHERE name = p_ensemble_type_name;
        
        IF v_ensemble_type_id IS NULL THEN
            RAISE EXCEPTION 'Тип ансамбля "%" не найден', p_ensemble_type_name;
        END IF;
    END IF;
    
    UPDATE shem.ensembles 
    SET 
        name = COALESCE(p_name, name),
        id_ensemble_types = COALESCE(v_ensemble_type_id, id_ensemble_types),
        founded_date = COALESCE(p_founded_date, founded_date),
        description = COALESCE(p_description, description)
    WHERE id_ensembles = p_id_ensembles;
    
    RAISE NOTICE 'Ансамбль с ID % успешно обновлен', p_id_ensembles;
END;
$$;

ALTER PROCEDURE shem.update_ensemble_info(INTEGER, VARCHAR, VARCHAR, DATE, TEXT)
    OWNER TO postgres;