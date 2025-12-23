CREATE OR REPLACE FUNCTION shem.sell_record(
    p_customer_id INTEGER,
    p_employee_id INTEGER, 
    p_record_id INTEGER,
    p_quantity INTEGER,
    p_payment_method_id INTEGER,
    p_purchase_date DATE DEFAULT CURRENT_DATE
) RETURNS TABLE(
    purchase_id INTEGER,
    purchase_date DATE,
    customer_name TEXT,
    employee_name TEXT, 
    record_title TEXT,
    quantity INTEGER,
    unit_price DECIMAL,
    total_amount DECIMAL,
    payment_method TEXT,
    remaining_stock INTEGER
) AS $$
DECLARE
    v_record_price DECIMAL(10,2);
    v_record_title VARCHAR(200);
    v_purchase_id INTEGER;
    v_current_stock INTEGER;
    v_total_amount DECIMAL(10,2);
    v_customer_name TEXT;
    v_employee_name TEXT;
    v_payment_method TEXT;
BEGIN
    SELECT retail_price, remaining_quantity, title 
    INTO v_record_price, v_current_stock, v_record_title
    FROM shem.record WHERE id_record = p_record_id;
    
    IF v_current_stock < p_quantity THEN
        RAISE EXCEPTION 'Недостаточно пластинок. Доступно: %, Запрошено: %', 
                        v_current_stock, p_quantity;
    END IF;
    
    SELECT first_name || ' ' || last_name INTO v_customer_name
    FROM shem.customers WHERE id_customers = p_customer_id;
    
    SELECT first_name || ' ' || last_name INTO v_employee_name
    FROM shem.employees WHERE id_employees = p_employee_id;
    
    SELECT name INTO v_payment_method
    FROM shem.payment_methods WHERE id_payment_methods = p_payment_method_id;
    
    v_total_amount := v_record_price * p_quantity;
    
    INSERT INTO shem.purchases (id_customers, id_employees, id_payment_methods, purchase_date)
    VALUES (p_customer_id, p_employee_id, p_payment_method_id, p_purchase_date)
    RETURNING id_purchases INTO v_purchase_id;
    
    INSERT INTO shem.purchase_details (id_purchases, id_record, quantity, unit_price)
    VALUES (v_purchase_id, p_record_id, p_quantity, v_record_price);
    
    UPDATE shem.record 
    SET remaining_quantity = remaining_quantity - p_quantity
    WHERE id_record = p_record_id
    RETURNING remaining_quantity INTO v_current_stock;
    
    RETURN QUERY SELECT 
        v_purchase_id,
        p_purchase_date,
        v_customer_name::TEXT,
        v_employee_name::TEXT,
        v_record_title::TEXT,
        p_quantity,
        v_record_price,
        v_total_amount,
        v_payment_method::TEXT,
        v_current_stock;
END;
$$ LANGUAGE plpgsql;