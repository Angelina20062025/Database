CREATE OR REPLACE PROCEDURE shem.delete_record(
    p_record_id INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_record_title VARCHAR;
    v_catalog_number VARCHAR;
BEGIN
    SELECT title, catalog_number 
    INTO v_record_title, v_catalog_number
    FROM shem.record 
    WHERE id_record = p_record_id;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Пластинка не найдена';
    END IF;
    
    IF EXISTS (
        SELECT 1 FROM shem.reservations 
        WHERE id_record = p_record_id AND status = 'Активно'
    ) THEN
        RAISE EXCEPTION 'Невозможно удалить "%" (кат. %). Есть активные бронирования.',
            v_record_title, v_catalog_number;
    END IF;
    
    DELETE FROM shem.reservations WHERE id_record = p_record_id;
    DELETE FROM shem.record_performances WHERE id_record = p_record_id;
    DELETE FROM shem.purchase_details WHERE id_record = p_record_id;
    
    DELETE FROM shem.record WHERE id_record = p_record_id;
    
    RAISE NOTICE 'Пластинка "%s" (кат. %s) успешно удалена', 
        v_record_title, v_catalog_number;
END;
$$;