CREATE OR REPLACE FUNCTION shem.soft_delete_composition(p_composition_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_composition_title VARCHAR;
    v_performances_count INTEGER;
BEGIN
    SELECT title INTO v_composition_title
    FROM shem.compositions 
    WHERE id_compositions = p_composition_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Композиция не найдена';
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO v_performances_count
    FROM shem.performances 
    WHERE id_compositions = p_composition_id;
    
    UPDATE shem.compositions 
    SET 
        is_deleted = TRUE
    WHERE id_compositions = p_composition_id;

	DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Композиция "%s" успешно архивирована.',
            v_composition_title
        );
        
        RETURN QUERY SELECT TRUE, v_summary;
    END;
END;
$$;