CREATE OR REPLACE FUNCTION shem.search_cds_by_ensemble(
    p_search_term VARCHAR(100)
)
RETURNS TABLE(
    cd_id INTEGER, 
    title VARCHAR(200), 
    catalog_number VARCHAR(50), 
    ensemble_name VARCHAR(100), 
    release_date DATE, 
    retail_price NUMERIC(10,2), 
    remaining_quantity INTEGER
) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT 
        r.id_record,
        r.title,
        r.catalog_number,
        e.name as ensemble_name,
        r.release_date,
        r.retail_price,
        r.remaining_quantity
    FROM shem.record r
    JOIN shem.record_performances rp ON r.id_record = rp.id_record
    JOIN shem.performances p ON rp.id_performances = p.id_performances
    JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles
    WHERE e.name ILIKE '%' || p_search_term || '%'
    ORDER BY e.name, r.release_date DESC;
END;
$$;