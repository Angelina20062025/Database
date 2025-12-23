ALTER TABLE shem.performances 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_performance(p_performance_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_performance_info TEXT;
    v_record_performances_count INTEGER;
BEGIN
    SELECT CONCAT(c.title, ' - ', e.name, ' (', p.performance_date, ')')
    INTO v_performance_info
    FROM shem.performances p
    JOIN shem.compositions c ON p.id_compositions = c.id_compositions
    JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles
    WHERE p.id_performances = p_performance_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Исполнение не найдено';
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO v_record_performances_count
    FROM shem.record_performances 
    WHERE id_performances = p_performance_id;
    
    UPDATE shem.performances 
    SET is_deleted = TRUE
    WHERE id_performances = p_performance_id;
    
    DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Исполнение "%s" успешно архивировано.' || E'\n' ||
            'Связанных пластинок: %s шт.',
            v_performance_info,
            v_record_performances_count
        );
        
        RETURN QUERY SELECT TRUE, v_summary;
    END;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.update_performance(
    p_id_performances INTEGER,
    p_composition_title VARCHAR(200) DEFAULT NULL,
    p_ensemble_name VARCHAR(100) DEFAULT NULL,
    p_performance_date DATE DEFAULT NULL,
    p_recording_location VARCHAR(100) DEFAULT NULL,
    p_record_ids INTEGER[] DEFAULT ARRAY[]::INTEGER[]
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_composition_id INTEGER;
    v_ensemble_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM shem.performances WHERE id_performances = p_id_performances) THEN
        RAISE EXCEPTION 'Исполнение с ID % не найдено', p_id_performances;
    END IF;
    
    IF p_composition_title IS NOT NULL THEN
        SELECT id_compositions INTO v_composition_id
        FROM shem.compositions 
        WHERE title = p_composition_title AND is_deleted = false;
        
        IF v_composition_id IS NULL THEN
            RAISE EXCEPTION 'Композиция "%" не найдена или архивирована', p_composition_title;
        END IF;
    END IF;
    
    IF p_ensemble_name IS NOT NULL THEN
        SELECT id_ensembles INTO v_ensemble_id
        FROM shem.ensembles 
        WHERE name = p_ensemble_name AND is_deleted = false;
        
        IF v_ensemble_id IS NULL THEN
            RAISE EXCEPTION 'Ансамбль "%" не найден или архивирован', p_ensemble_name;
        END IF;
    END IF;
    
    IF p_performance_date IS NOT NULL AND p_performance_date > CURRENT_DATE THEN
        RAISE EXCEPTION 'Дата исполнения не может быть в будущем';
    END IF;
    
    UPDATE shem.performances 
    SET 
        id_compositions = COALESCE(v_composition_id, id_compositions),
        id_ensembles = COALESCE(v_ensemble_id, id_ensembles),
        performance_date = COALESCE(p_performance_date, performance_date),
        recording_location = COALESCE(p_recording_location, recording_location)
    WHERE id_performances = p_id_performances;
    
    DELETE FROM shem.record_performances 
    WHERE id_performances = p_id_performances;
    
    IF p_record_ids IS NOT NULL AND array_length(p_record_ids, 1) > 0 THEN
        FOR i IN 1..array_length(p_record_ids, 1) LOOP
            IF EXISTS (SELECT 1 FROM shem.record WHERE id_record = p_record_ids[i] AND is_deleted = false) THEN
                INSERT INTO shem.record_performances (id_record, id_performances)
                VALUES (p_record_ids[i], p_id_performances);
            END IF;
        END LOOP;
    END IF;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.insert_performance(
    p_composition_title VARCHAR(200),
    p_ensemble_name VARCHAR(100),
    p_performance_date DATE,
    p_recording_location VARCHAR(100),
    p_record_ids INTEGER[] DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_composition_id INTEGER;
    v_ensemble_id INTEGER;
    v_performance_id INTEGER;
BEGIN
    SELECT id_compositions INTO v_composition_id
    FROM shem.compositions 
    WHERE title = p_composition_title;
    
    IF v_composition_id IS NULL THEN
        RAISE EXCEPTION 'Композиция "%" не найдена', p_composition_title;
    END IF;
    
    SELECT id_ensembles INTO v_ensemble_id
    FROM shem.ensembles 
    WHERE name = p_ensemble_name AND is_deleted = false;
    
    IF v_ensemble_id IS NULL THEN
        RAISE EXCEPTION 'Ансамбль "%" не найден или архивирован', p_ensemble_name;
    END IF;
    
    IF p_performance_date > CURRENT_DATE THEN
        RAISE EXCEPTION 'Дата исполнения не может быть дальше сегодняшней';
    END IF;
    
    INSERT INTO shem.performances (
        id_compositions,
        id_ensembles,
        performance_date,
        recording_location
    ) VALUES (
        v_composition_id,
        v_ensemble_id,
        p_performance_date,
        p_recording_location
    ) RETURNING id_performances INTO v_performance_id;
    
    IF p_record_ids IS NOT NULL AND array_length(p_record_ids, 1) > 0 THEN
        FOR i IN 1..array_length(p_record_ids, 1) LOOP
            IF NOT EXISTS (SELECT 1 FROM shem.record WHERE id_record = p_record_ids[i] AND is_deleted = false) THEN
                RAISE NOTICE 'Пластинка с ID % не найдена или архивирована', p_record_ids[i];
                CONTINUE;
            END IF;
            
            INSERT INTO shem.record_performances (id_record, id_performances)
            VALUES (p_record_ids[i], v_performance_id);
        END LOOP;
    END IF;
END;
$$;