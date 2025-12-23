ALTER TABLE shem.record 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_record(p_record_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT,
    active_reservations INTEGER,
    total_sales INTEGER,
    performances_count INTEGER
) AS $$
DECLARE
    v_record_title VARCHAR;
    v_catalog_number VARCHAR;
    v_active_reservations INTEGER;
    v_total_sales INTEGER;
    v_performances_count INTEGER;
BEGIN
    SELECT title, catalog_number 
    INTO v_record_title, v_catalog_number
    FROM shem.record 
    WHERE id_record = p_record_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Пластинка не найдена', 0, 0, 0;
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO v_active_reservations
    FROM shem.reservations 
    WHERE id_record = p_record_id AND status = 'Активно';
    
    SELECT COUNT(*) INTO v_total_sales
    FROM shem.purchase_details 
    WHERE id_record = p_record_id;
    
    SELECT COUNT(*) INTO v_performances_count
    FROM shem.record_performances 
    WHERE id_record = p_record_id;
    
    IF v_active_reservations > 0 THEN
        RETURN QUERY SELECT 
            FALSE,
            FORMAT(
                'Невозможно архивировать "%s" (%s). Есть активные бронирования: %s шт.',
                v_record_title, v_catalog_number, v_active_reservations
            ),
            v_active_reservations, v_total_sales, v_performances_count;
        RETURN;
    END IF;
    
    UPDATE shem.record 
    SET 
        is_deleted = TRUE,
        remaining_quantity = 0
    WHERE id_record = p_record_id;
    
    DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Пластинка "%s" (%s) архивирована.' || E'\n' ||
            'Данные о пластинке:' || E'\n' ||
            'Продаж в истории: %s шт.' || E'\n' ||
            'Связанных исполнений: %s шт.',
            v_record_title, v_catalog_number,
            v_total_sales, v_performances_count
        );
        
        RETURN QUERY SELECT 
            TRUE,
            v_summary,
            v_active_reservations,
            v_total_sales,
            v_performances_count;
    END;
END;
$$ LANGUAGE plpgsql;