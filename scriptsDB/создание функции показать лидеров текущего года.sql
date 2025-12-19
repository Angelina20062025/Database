CREATE OR REPLACE FUNCTION shem.get_sales_leaders(
    p_limit INTEGER DEFAULT 10
)
RETURNS TABLE(
	id_record INTEGER,
    cd_title VARCHAR(200),
    catalog_number VARCHAR(50),
    current_year INTEGER,
    total_revenue NUMERIC(12,2),
    last_year_sales BIGINT,
    remaining_quantity INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
		rss.id_record,
        rss.title,
        rss.catalog_number,
        rss.current_year_sales::INTEGER as current_year,
        (rss.current_year_sales * rss.retail_price)::NUMERIC(12,2) as total_revenue,
        rss.last_year_sales,
        rss.remaining_quantity
    FROM shem.record_sales_stats rss
    WHERE rss.current_year_sales > 0
    ORDER BY rss.current_year_sales DESC, total_revenue DESC
    LIMIT p_limit;
END;
$$;