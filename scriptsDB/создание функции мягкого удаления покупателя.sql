ALTER TABLE shem.customers 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_customer(p_customer_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_customer_name TEXT;
    v_active_reservations INTEGER;
    v_total_purchases INTEGER;
    v_total_amount NUMERIC;
BEGIN
    
    SELECT first_name || ' ' || last_name INTO v_customer_name
    FROM shem.customers 
    WHERE id_customers = p_customer_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Покупатель не найден';
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO v_active_reservations
    FROM shem.reservations 
    WHERE id_customers = p_customer_id AND status = 'Активно';
    
    SELECT COUNT(*), COALESCE(SUM(pd.quantity * pd.unit_price), 0)
    INTO v_total_purchases, v_total_amount
    FROM shem.purchases p
    JOIN shem.purchase_details pd ON p.id_purchases = pd.id_purchases
    WHERE p.id_customers = p_customer_id;
    
    IF v_active_reservations > 0 THEN
        RETURN QUERY SELECT 
            FALSE,
            FORMAT(
                'Невозможно архивировать покупателя "%s". Есть активные бронирования: %s шт.',
                v_customer_name, v_active_reservations
            );
        RETURN;
    END IF;
    
    UPDATE shem.customers 
    SET 
        is_deleted = TRUE
    WHERE id_customers = p_customer_id;
    
    DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Покупатель "%s" успешно архивирован.' || E'\n' ||
            'Количество покупок: %s шт.' || E'\n' ||
            'Общая сумма покупок: %s руб.',
            v_customer_name,
            v_total_purchases,
            v_total_amount
        );
        
        RETURN QUERY SELECT TRUE, v_summary;
    END;
END;
$$;