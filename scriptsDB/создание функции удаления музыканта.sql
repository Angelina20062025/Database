ALTER TABLE shem.musicians 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_musician(p_musician_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_musician_name TEXT;
    v_ensembles_count INTEGER;
    v_performances_count INTEGER;
BEGIN
    SELECT first_name || ' ' || last_name INTO v_musician_name
    FROM shem.musicians 
    WHERE id_musicians = p_musician_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Музыкант не найден';
        RETURN;
    END IF;
    
    SELECT COUNT(DISTINCT id_ensembles) INTO v_ensembles_count
    FROM shem.ensemble_members 
    WHERE id_musicians = p_musician_id;
    
    SELECT COUNT(DISTINCT p.id_performances) INTO v_performances_count
    FROM shem.ensemble_members em
    JOIN shem.performances p ON em.id_ensembles = p.id_ensembles
    WHERE em.id_musicians = p_musician_id;
    
    UPDATE shem.musicians 
    SET 
        is_deleted = TRUE
    WHERE id_musicians = p_musician_id;
    
    DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Музыкант "%s" успешно архивирован.' || E'\n' ||
            'Состоял в ансамблях: %s шт.' || E'\n' ||
            'Участвовал в исполнениях: %s шт.',
            v_musician_name,
            v_ensembles_count,
            v_performances_count
        );
        
        RETURN QUERY SELECT TRUE, v_summary;
    END;
END;
$$;