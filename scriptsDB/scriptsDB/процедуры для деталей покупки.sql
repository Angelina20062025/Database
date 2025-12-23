CREATE OR REPLACE FUNCTION shem.delete_purchase_detail(p_id_purchase_details INTEGER)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    returned_quantity INTEGER,
    record_title VARCHAR
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_quantity INTEGER;
    v_record_id INTEGER;
    v_purchase_id INTEGER;
    v_record_title VARCHAR;
    v_purchase_deleted BOOLEAN;
BEGIN
    SELECT pd.quantity, pd.id_record, pd.id_purchases, r.title
    INTO v_quantity, v_record_id, v_purchase_id, v_record_title
    FROM shem.purchase_details pd
    JOIN shem.record r ON pd.id_record = r.id_record
    WHERE pd.id_purchase_details = p_id_purchase_details;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Деталь покупки не найдена', 0, '';
        RETURN;
    END IF;
    
    SELECT is_deleted INTO v_purchase_deleted
    FROM shem.purchases 
    WHERE id_purchases = v_purchase_id;
    
    IF v_purchase_deleted THEN
        RETURN QUERY SELECT FALSE, 'Покупка архивирована, нельзя удалять детали', 0, '';
        RETURN;
    END IF;
    
    BEGIN
        DELETE FROM shem.purchase_details 
        WHERE id_purchase_details = p_id_purchase_details;
        
        UPDATE shem.record 
        SET remaining_quantity = remaining_quantity + v_quantity
        WHERE id_record = v_record_id;
        
        RETURN QUERY SELECT TRUE, 
            FORMAT('Деталь покупки удалена. Возвращено товаров: %s шт.', v_quantity),
            v_quantity, v_record_title;
        
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.update_purchase_detail(
    p_id_purchase_details INTEGER,
    p_quantity INTEGER DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_old_quantity INTEGER;
    v_record_id INTEGER;
    v_purchase_id INTEGER;
    v_unit_price NUMERIC(10,2);
    v_record_title VARCHAR;
    v_current_stock INTEGER;
    v_quantity_diff INTEGER;
BEGIN
    SELECT pd.quantity, pd.id_record, pd.id_purchases, pd.unit_price, r.title, r.remaining_quantity
    INTO v_old_quantity, v_record_id, v_purchase_id, v_unit_price, v_record_title, v_current_stock
    FROM shem.purchase_details pd
    JOIN shem.record r ON pd.id_record = r.id_record
    WHERE pd.id_purchase_details = p_id_purchase_details;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Деталь покупки % не найдена', p_id_purchase_details;
    END IF;
    
    IF EXISTS (SELECT 1 FROM shem.purchases WHERE id_purchases = v_purchase_id AND is_deleted = true) THEN
        RAISE EXCEPTION 'Покупка архивирована, нельзя изменять детали';
    END IF;
    
    IF p_quantity IS NULL THEN
        p_quantity := v_old_quantity;
    END IF;
    
    IF p_quantity <= 0 THEN
        RAISE EXCEPTION 'Количество должно быть положительным числом';
    END IF;
    
    v_quantity_diff := p_quantity - v_old_quantity;
    
    IF v_quantity_diff > 0 AND v_current_stock < v_quantity_diff THEN
        RAISE EXCEPTION 'Недостаточно пластинок "%". Доступно: %, Требуется дополнительно: %', 
            v_record_title, v_current_stock, v_quantity_diff;
    END IF;
    
    BEGIN
        -- Обновляем деталь покупки
        UPDATE shem.purchase_details 
        SET quantity = p_quantity
        WHERE id_purchase_details = p_id_purchase_details;
        
        UPDATE shem.record 
        SET remaining_quantity = remaining_quantity - v_quantity_diff
        WHERE id_record = v_record_id;
        
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.insert_purchase_detail(
    p_purchase_id INTEGER,
    p_record_id INTEGER,
    p_quantity INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_purchase_exists BOOLEAN;
    v_record_exists BOOLEAN;
    v_current_stock INTEGER;
    v_unit_price NUMERIC(10,2);
    v_record_title VARCHAR;
BEGIN
    SELECT EXISTS(
        SELECT 1 FROM shem.purchases 
        WHERE id_purchases = p_purchase_id AND is_deleted = false
    ) INTO v_purchase_exists;
    
    IF NOT v_purchase_exists THEN
        RAISE EXCEPTION 'Покупка % не найдена или архивирована', p_purchase_id;
    END IF;
    
    SELECT EXISTS(
        SELECT 1 FROM shem.record 
        WHERE id_record = p_record_id AND is_deleted = false
    ), remaining_quantity, retail_price, title 
    INTO v_record_exists, v_current_stock, v_unit_price, v_record_title
    FROM shem.record 
    WHERE id_record = p_record_id;
    
    IF p_quantity <= 0 THEN
        RAISE EXCEPTION 'Количество должно быть положительным числом';
    END IF;
    
    IF v_current_stock < p_quantity THEN
        RAISE EXCEPTION 'Недостаточно пластинок "%". Доступно: %, Запрошено: %', 
            v_record_title, v_current_stock, p_quantity;
    END IF;
    
    BEGIN
        INSERT INTO shem.purchase_details (
            id_purchases,
            id_record,
            quantity,
            unit_price
        ) VALUES (
            p_purchase_id,
            p_record_id,
            p_quantity,
            v_unit_price
        );
        
        UPDATE shem.record 
        SET remaining_quantity = remaining_quantity - p_quantity
        WHERE id_record = p_record_id;
        
        RAISE NOTICE 'Деталь покупки успешно добавлена. Пластинка: %, Количество: %', 
            v_record_title, p_quantity;
        
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END;
END;
$$;