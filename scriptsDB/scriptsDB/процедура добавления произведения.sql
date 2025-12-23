CREATE OR REPLACE PROCEDURE shem.insert_composition(
    p_title VARCHAR(200),
    p_genre_name VARCHAR(100),
    p_duration_seconds INTEGER,
    p_year_created INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_genre_id INTEGER;
BEGIN
    SELECT id_genres INTO v_genre_id
    FROM shem.genres 
    WHERE name = p_genre_name;
    
    IF v_genre_id IS NULL THEN
        RAISE EXCEPTION 'Жанр "%" не найден', p_genre_name;
    END IF;
    
    INSERT INTO shem.compositions (
        title, 
        id_genres, 
        duration_seconds, 
        year_created
    ) VALUES (
        p_title,
        v_genre_id,
        p_duration_seconds,
        p_year_created
    );
END;
$$;

ALTER TABLE shem.compositions 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;