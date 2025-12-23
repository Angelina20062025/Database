ALTER TABLE shem.reservations 
ADD COLUMN is_deleted BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION shem.soft_delete_reservation(p_reservation_id INTEGER)
RETURNS TABLE(
    success BOOLEAN, 
    message TEXT
) 
LANGUAGE plpgsql
AS $$
DECLARE
    v_reservation_info TEXT;
    v_current_status VARCHAR(20);
BEGIN
    SELECT CONCAT(c.first_name, ' ', c.last_name, ' - ', r.title, ' (', res.quantity, ' шт.)'),
           res.status
    INTO v_reservation_info, v_current_status
    FROM shem.reservations res
    JOIN shem.customers c ON res.id_customers = c.id_customers
    JOIN shem.record r ON res.id_record = r.id_record
    WHERE res.id_reservations = p_reservation_id;
    
    IF NOT FOUND THEN
        RETURN QUERY SELECT FALSE, 'Бронирование не найдено';
        RETURN;
    END IF;
    
    IF EXISTS (SELECT 1 FROM shem.reservations WHERE id_reservations = p_reservation_id AND is_deleted = TRUE) THEN
        RETURN QUERY SELECT FALSE, 'Бронирование уже архивировано';
        RETURN;
    END IF;
    
    IF v_current_status = 'Активно' THEN
        RETURN QUERY SELECT FALSE, 'Невозможно архивировать активное бронирование.';
        RETURN;
    END IF;
    
    UPDATE shem.reservations 
    SET is_deleted = TRUE
    WHERE id_reservations = p_reservation_id;
    
    DECLARE
        v_summary TEXT;
    BEGIN
        v_summary := FORMAT(
            'Бронирование "%s" успешно архивировано.',
            v_reservation_info
        );
        
        RETURN QUERY SELECT TRUE, v_summary;
    END;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.insert_reservation(
    p_customer_id INTEGER,
    p_employee_id INTEGER,
    p_record_id INTEGER,
    p_quantity INTEGER,
    p_expiry_days INTEGER DEFAULT 3,
    p_notes TEXT DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_reservation_date DATE := CURRENT_DATE;
    v_expiry_date DATE;
    v_record_title VARCHAR;
    v_current_stock INTEGER;
BEGIN
    SELECT title, remaining_quantity INTO v_record_title, v_current_stock
    FROM shem.record 
    WHERE id_record = p_record_id AND is_deleted = false;
    
    IF p_quantity <= 0 THEN
        RAISE EXCEPTION 'Количество должно быть положительным числом';
    END IF;
    
    IF v_current_stock < p_quantity THEN
        RAISE EXCEPTION 'Недостаточно пластинок "%". Доступно: %, Запрошено: %', 
            v_record_title, v_current_stock, p_quantity;
    END IF;
    
    IF p_expiry_days <= 0 OR p_expiry_days > 7 THEN
        RAISE EXCEPTION 'Срок бронирования должен быть от 1 до 7 дней';
    END IF;
    
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
        'Активно',
        p_quantity,
        v_expiry_date,
        p_notes
    );
    
    UPDATE shem.record 
    SET remaining_quantity = remaining_quantity - p_quantity
    WHERE id_record = p_record_id;
END;
$$;

CREATE OR REPLACE PROCEDURE shem.update_reservation(
    p_id_reservations INTEGER,
    p_customer_id INTEGER DEFAULT NULL,
    p_employee_id INTEGER DEFAULT NULL,
    p_record_id INTEGER DEFAULT NULL,
    p_quantity INTEGER DEFAULT NULL,
    p_expiry_date DATE DEFAULT NULL,
    p_notes TEXT DEFAULT NULL,
    p_status VARCHAR(20) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_quantity INTEGER;
    v_current_record_id INTEGER;
    v_current_status VARCHAR(20);
    v_new_record_id INTEGER;
    v_quantity_diff INTEGER;
    v_current_stock INTEGER;
    v_record_title VARCHAR;
BEGIN
    SELECT quantity, id_record, status 
    INTO v_current_quantity, v_current_record_id, v_current_status
    FROM shem.reservations 
    WHERE id_reservations = p_id_reservations;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Бронирование % не найдено', p_id_reservations;
    END IF;
    
    v_new_record_id := COALESCE(p_record_id, v_current_record_id);
    
    IF p_customer_id IS NOT NULL THEN
        IF NOT EXISTS (SELECT 1 FROM shem.customers WHERE id_customers = p_customer_id AND is_deleted = false) THEN
            RAISE EXCEPTION 'Покупатель с ID % не найден или архивирован', p_customer_id;
        END IF;
    END IF;
    
    IF p_employee_id IS NOT NULL THEN
        IF NOT EXISTS (SELECT 1 FROM shem.employees WHERE id_employees = p_employee_id) THEN
            RAISE EXCEPTION 'Сотрудник с ID % не найден', p_employee_id;
        END IF;
    END IF;
    
    IF p_record_id IS NOT NULL AND p_record_id != v_current_record_id THEN
        SELECT title, remaining_quantity INTO v_record_title, v_current_stock
        FROM shem.record 
        WHERE id_record = p_record_id AND is_deleted = false;
        
        IF NOT FOUND THEN
            RAISE EXCEPTION 'Пластинка % не найдена или архивирована', p_record_id;
        END IF;
    END IF;
    
    IF p_quantity IS NOT NULL THEN
        IF p_quantity <= 0 THEN
            RAISE EXCEPTION 'Количество должно быть положительным числом';
        END IF;
        
        SELECT title, remaining_quantity INTO v_record_title, v_current_stock
        FROM shem.record 
        WHERE id_record = v_new_record_id;
        
        v_quantity_diff := p_quantity - v_current_quantity;
        
        IF v_quantity_diff > 0 AND v_current_stock < v_quantity_diff THEN
            RAISE EXCEPTION 'Недостаточно пластинок "%". Доступно: %, Требуется дополнительно: %', 
                v_record_title, v_current_stock, v_quantity_diff;
        END IF;
    END IF;
    
    IF p_status IS NOT NULL THEN
        IF p_status = 'Отменено' AND v_current_status != 'Отменено' THEN
            UPDATE shem.record 
            SET remaining_quantity = remaining_quantity + v_current_quantity
            WHERE id_record = v_current_record_id;
        END IF;
        
        IF v_current_status = 'Отменено' AND p_status != 'Отменено' THEN
            SELECT remaining_quantity INTO v_current_stock
            FROM shem.record 
            WHERE id_record = v_new_record_id;
            
            IF COALESCE(p_quantity, v_current_quantity) > v_current_stock THEN
                RAISE EXCEPTION 'Недостаточно товара на складе для восстановления брони';
            END IF;
            
            UPDATE shem.record 
            SET remaining_quantity = remaining_quantity - COALESCE(p_quantity, v_current_quantity)
            WHERE id_record = v_new_record_id;
        END IF;
    END IF;
    
    BEGIN
        UPDATE shem.reservations 
        SET 
            id_customers = COALESCE(p_customer_id, id_customers),
            id_employees = COALESCE(p_employee_id, id_employees),
            id_record = COALESCE(p_record_id, id_record),
            quantity = COALESCE(p_quantity, quantity),
            expiry_date = COALESCE(p_expiry_date, expiry_date),
            notes = COALESCE(p_notes, notes),
            status = COALESCE(p_status, status)
        WHERE id_reservations = p_id_reservations;
        
        IF p_quantity IS NOT NULL AND v_quantity_diff != 0 THEN
            UPDATE shem.record 
            SET remaining_quantity = remaining_quantity - v_quantity_diff
            WHERE id_record = v_new_record_id;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END;
END;
$$;

CREATE OR REPLACE FUNCTION check_reservation_duration()
RETURNS TRIGGER AS $$
BEGIN
    IF (NEW.expiry_date - NEW.reservation_date) > 7 THEN
        RAISE EXCEPTION 'Срок бронирования не может превышать 7 дней. Попытка установить срок: % дней', 
                       (NEW.expiry_date - NEW.reservation_date);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_check_reservation_duration
    BEFORE INSERT OR UPDATE ON shem.reservations
    FOR EACH ROW
    EXECUTE FUNCTION check_reservation_duration();