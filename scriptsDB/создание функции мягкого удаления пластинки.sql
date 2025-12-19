CREATE OR REPLACE FUNCTION shem.check_record_before_delete()
RETURNS TRIGGER AS $$
DECLARE
    v_active_reservations INTEGER;
    v_total_sales INTEGER;
    v_performances_count INTEGER;
    v_message TEXT;
BEGIN
    SELECT COUNT(*) INTO v_active_reservations
    FROM shem.reservations 
    WHERE id_record = OLD.id_record AND status = 'Активно';
    
    SELECT COUNT(*) INTO v_total_sales
    FROM shem.purchase_details 
    WHERE id_record = OLD.id_record;
    
    SELECT COUNT(*) INTO v_performances_count
    FROM shem.record_performances 
    WHERE id_record = OLD.id_record;
	
    v_message := '';
    
    IF v_active_reservations > 0 THEN
        v_message := v_message || FORMAT(
            'Активных бронирований: %s шт.' || E'\n',
            v_active_reservations
        );
    END IF;
    
    IF v_total_sales > 0 THEN
        v_message := v_message || FORMAT(
            'Продаж: %s шт.' || E'\n',
            v_total_sales
        );
    END IF;
    
    IF v_performances_count > 0 THEN
        v_message := v_message || FORMAT(
            'Записанных исполнений: %s шт.' || E'\n',
            v_performances_count
        );
    END IF;
    
    IF v_active_reservations > 0 OR v_total_sales > 0 OR 
       v_performances_count > 0 THEN
        
        RAISE EXCEPTION 
            'Невозможно удалить пластинку "%s" (кат. %s). 
			Обнаружены связанные записи: % 
			Сначала удалите связанные записи.',
            OLD.title,
            OLD.catalog_number,
            v_message;
    END IF;
    
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

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