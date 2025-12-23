CREATE OR REPLACE FUNCTION shem.get_ensemble_cds(
    p_ensemble_name VARCHAR(100)
)
RETURNS TABLE(
    cd_title VARCHAR(200),
    catalog_number VARCHAR(50),
    release_date DATE
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_ensemble_id INTEGER;
BEGIN
    SELECT id_ensembles INTO v_ensemble_id
    FROM shem.ensembles
    WHERE name = p_ensemble_name;
    
    IF v_ensemble_id IS NULL THEN
        cd_title := 'Ансамбль не найден: ' || p_ensemble_name;
        catalog_number := '';
        release_date := NULL;
        RETURN NEXT;
        RETURN;
    END IF;
    
    IF NOT EXISTS (
        SELECT 1 
        FROM shem.record r
        JOIN shem.record_performances rp ON r.id_record = rp.id_record
        JOIN shem.performances p ON rp.id_performances = p.id_performances
        WHERE p.id_ensembles = v_ensemble_id
    ) THEN
        cd_title := 'У ансамбля ' || p_ensemble_name || ' нет дисков';
        catalog_number := '';
        release_date := NULL;
        RETURN NEXT;
        RETURN;
    END IF;
    
    RETURN QUERY
    SELECT DISTINCT 
        r.title,
        r.catalog_number,
        r.release_date
    FROM shem.record r
    JOIN shem.record_performances rp ON r.id_record = rp.id_record
    JOIN shem.performances p ON rp.id_performances = p.id_performances
    JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles
    WHERE e.name = p_ensemble_name
    ORDER BY r.release_date DESC;
END;
$$;