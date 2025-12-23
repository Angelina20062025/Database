ALTER TABLE shem.purchases 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_purchase(p_purchase_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT,
    details_count INTEGER,
    total_amount NUMERIC(10,2)
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_purchase_exists BOOLEAN;
    v_purchase_date DATE;
    v_details_count INTEGER;
    v_total_amount NUMERIC(10,2);
BEGIN
    SELECT EXISTS(
        SELECT 1 FROM shem.purchases 
        WHERE id_purchases = p_purchase_id
    ), purchase_date INTO v_purchase_exists, v_purchase_date
    FROM shem.purchases 
    WHERE id_purchases = p_purchase_id;
    
    IF NOT v_purchase_exists THEN
        RETURN QUERY SELECT FALSE, 'Покупка не найдена', 0, 0;
        RETURN;
    END IF;
    
    SELECT COUNT(*), COALESCE(SUM(quantity * unit_price), 0)
    INTO v_details_count, v_total_amount
    FROM shem.purchase_details 
    WHERE id_purchases = p_purchase_id;
    
    IF v_details_count > 0 THEN
        RETURN QUERY SELECT FALSE, 
            FORMAT('Невозможно архивировать покупку. Сначала удалите детали покупки (%s шт.)', v_details_count),
            v_details_count, v_total_amount;
        RETURN;
    END IF;
    
    UPDATE shem.purchases 
    SET is_deleted = TRUE
    WHERE id_purchases = p_purchase_id;
    
    RETURN QUERY SELECT TRUE, 
        FORMAT('Покупка успешно архивирована'),
        v_details_count, v_total_amount;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.update_purchase(
    p_id_purchases INTEGER,
    p_customer_id INTEGER DEFAULT NULL,
    p_employee_id INTEGER DEFAULT NULL,
    p_payment_method_name VARCHAR(50) DEFAULT NULL,
    p_purchase_date DATE DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_payment_method_id INTEGER;
BEGIN
    IF p_payment_method_name IS NOT NULL THEN
        SELECT id_payment_methods INTO v_payment_method_id
        FROM shem.payment_methods 
        WHERE name = p_payment_method_name;
    END IF;
    
    UPDATE shem.purchases 
    SET 
        id_customers = COALESCE(p_customer_id, id_customers),
        id_employees = COALESCE(p_employee_id, id_employees),
        id_payment_methods = COALESCE(v_payment_method_id, id_payment_methods),
        purchase_date = COALESCE(p_purchase_date, purchase_date)
    WHERE id_purchases = p_id_purchases;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.insert_purchase(
    p_customer_id INTEGER,
    p_employee_id INTEGER,
    p_payment_method_name VARCHAR(50),
    p_purchase_date DATE DEFAULT CURRENT_DATE
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_payment_method_id INTEGER;
BEGIN
    SELECT id_payment_methods INTO v_payment_method_id
    FROM shem.payment_methods 
    WHERE name = p_payment_method_name;
    
    INSERT INTO shem.purchases (
        id_customers,
        id_employees,
        id_payment_methods,
        purchase_date
    ) VALUES (
        p_customer_id,
        p_employee_id,
        v_payment_method_id,
        p_purchase_date
    );
END;
$$;