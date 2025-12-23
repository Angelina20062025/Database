SELECT 
    c.title,
    g.name as genre_name,
    c.duration_seconds
FROM shem.compositions c
JOIN shem.genres g ON c.id_genres = g.id_genres;