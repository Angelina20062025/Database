CREATE OR REPLACE FUNCTION shem.get_ensemble_compositions_count(
    p_ensemble_name VARCHAR(100)
)
RETURNS TABLE(
    ensemble_name VARCHAR(100),
    compositions_count INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        e.name as ensemble_name,
        COUNT(DISTINCT p.id_compositions)::INTEGER as compositions_count
    FROM shem.ensembles e
    LEFT JOIN shem.performances p ON e.id_ensembles = p.id_ensembles
    WHERE e.name = p_ensemble_name
    GROUP BY e.id_ensembles, e.name;
    
    IF NOT FOUND THEN
        RETURN QUERY
        SELECT p_ensemble_name::VARCHAR(100) as ensemble_name, 0::INTEGER as compositions_count;
    END IF;
END;
$$;