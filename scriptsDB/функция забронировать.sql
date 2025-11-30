CREATE OR REPLACE FUNCTION shem.create_reservation(
    p_customer_id INTEGER,
    p_employee_id INTEGER,
    p_record_id INTEGER,
    p_quantity INTEGER,
    p_expiry_days INTEGER DEFAULT 3,
    p_notes TEXT DEFAULT NULL
) RETURNS TABLE(
    reservation_id INTEGER,
    customer_name TEXT,
    employee_name TEXT,
    record_title TEXT,
    quantity INTEGER,
    reservation_date DATE,
    expiry_date DATE,
    status TEXT,
    notes TEXT
) AS $$
DECLARE
    v_record_title VARCHAR(200);
    v_reservation_id INTEGER;
    v_current_stock INTEGER;
    v_customer_name TEXT;
    v_employee_name TEXT;
    v_reservation_date DATE := CURRENT_DATE;
    v_expiry_date DATE;
BEGIN
    SELECT title, remaining_quantity 
    INTO v_record_title, v_current_stock
    FROM shem.record WHERE id_record = p_record_id;
    
    IF v_current_stock < p_quantity THEN
        RAISE EXCEPTION 'Недостаточно пластинок для брони. Доступно: %, Запрошено: %', 
                        v_current_stock, p_quantity;
    END IF;
    
    SELECT first_name || ' ' || last_name INTO v_customer_name
    FROM shem.customers WHERE id_customers = p_customer_id;
    
    SELECT first_name || ' ' || last_name INTO v_employee_name
    FROM shem.employees WHERE id_employees = p_employee_id;
    
    v_expiry_date := v_reservation_date + (p_expiry_days || ' days')::INTERVAL;
    
    INSERT INTO shem.reservations (
        id_customers, 
        id_employees, 
        id_record, 
        reservation_date, 
        status, 
        quantity, 
        expiry_date, 
        notes
    ) VALUES (
        p_customer_id,
        p_employee_id,
        p_record_id,
        v_reservation_date,
        'Активная бронь',
        p_quantity,
        v_expiry_date,
        p_notes
    ) RETURNING id_reservations INTO v_reservation_id;
    
    UPDATE shem.record 
    SET remaining_quantity = remaining_quantity - p_quantity
    WHERE id_record = p_record_id;
    
    RETURN QUERY SELECT 
        v_reservation_id,
        v_customer_name::TEXT,
        v_employee_name::TEXT,
        v_record_title::TEXT,
        p_quantity,
        v_reservation_date,
        v_expiry_date,
        'Активная бронь'::TEXT,
        COALESCE(p_notes, '')::TEXT;
END;
$$ LANGUAGE plpgsql;