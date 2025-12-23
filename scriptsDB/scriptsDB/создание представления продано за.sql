CREATE OR REPLACE VIEW shem.record_sales_stats AS
SELECT 
    r.id_record,
    r.catalog_number,
    r.title,
    r.remaining_quantity,
    r.retail_price,
    (SELECT COALESCE(SUM(pd.quantity), 0)
     FROM shem.purchase_details pd
     JOIN shem.purchases p ON pd.id_purchases = p.id_purchases
     WHERE pd.id_record = r.id_record
     AND EXTRACT(YEAR FROM p.purchase_date) = EXTRACT(YEAR FROM CURRENT_DATE)
    ) as current_year_sales,
    (SELECT COALESCE(SUM(pd.quantity), 0)
     FROM shem.purchase_details pd
     JOIN shem.purchases p ON pd.id_purchases = p.id_purchases
     WHERE pd.id_record = r.id_record
     AND EXTRACT(YEAR FROM p.purchase_date) = EXTRACT(YEAR FROM CURRENT_DATE) - 1
    ) as last_year_sales,
    (SELECT COALESCE(SUM(pd.quantity), 0)
     FROM shem.purchase_details pd
     WHERE pd.id_record = r.id_record
    ) as total_sales
FROM shem.record r;