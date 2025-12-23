CREATE OR REPLACE PROCEDURE shem.update_composition(
    p_id_compositions INTEGER,
    p_title VARCHAR(200) DEFAULT NULL,
    p_genre_name VARCHAR(100) DEFAULT NULL,
    p_duration_seconds INTEGER DEFAULT NULL,
    p_year_created INTEGER DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_genre_id INTEGER;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM shem.compositions WHERE id_compositions = p_id_compositions) THEN
        RAISE EXCEPTION 'Композиция % не найдена', p_id_compositions;
    END IF;
    
    IF p_genre_name IS NOT NULL THEN
        SELECT id_genres INTO v_genre_id
        FROM shem.genres 
        WHERE name = p_genre_name;
        
        IF v_genre_id IS NULL THEN
            RAISE EXCEPTION 'Жанр "%" не найден', p_genre_name;
        END IF;
    END IF;
    
    UPDATE shem.compositions 
    SET 
        title = COALESCE(p_title, title),
        id_genres = COALESCE(v_genre_id, id_genres),
        duration_seconds = COALESCE(p_duration_seconds, duration_seconds),
        year_created = COALESCE(p_year_created, year_created)
    WHERE id_compositions = p_id_compositions;
END;
$$;