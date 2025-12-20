ALTER TABLE shem.ensembles 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_ensemble(p_ensemble_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_ensemble_name VARCHAR;
    v_members_count INTEGER;
    v_performances_count INTEGER;
    v_record_performances_count INTEGER;
BEGIN
    
    SELECT name INTO v_ensemble_name
    FROM shem.ensembles 
    WHERE id_ensembles = p_ensemble_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Ансамбль не найден';
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO v_members_count
    FROM shem.ensemble_members 
    WHERE id_ensembles = p_ensemble_id;
    
    SELECT COUNT(*) INTO v_performances_count
    FROM shem.performances 
    WHERE id_ensembles = p_ensemble_id;
    
    SELECT COUNT(*) INTO v_record_performances_count
    FROM shem.record_performances rp
    JOIN shem.performances p ON rp.id_performances = p.id_performances
    WHERE p.id_ensembles = p_ensemble_id;
    
    UPDATE shem.ensembles 
    SET 
        is_deleted = TRUE
    WHERE id_ensembles = p_ensemble_id;
    
    DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Ансамбль "%s" успешно архивирован.' || E'\n' ||
            'Участников: %s чел.' || E'\n' ||
            'Исполнений: %s шт.' || E'\n' ||
            'Связанных пластинок: %s шт.',
            v_ensemble_name,
            v_members_count,
            v_performances_count,
            v_record_performances_count
        );
        
        RETURN QUERY SELECT TRUE, v_summary;
    END;
END;
$$;