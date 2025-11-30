--для завершения брони (при продаже)
CREATE OR REPLACE FUNCTION shem.complete_reservation(
    p_reservation_id INTEGER
) RETURNS VOID AS $$
BEGIN
    UPDATE shem.reservations 
    SET status = 'Завершено'
    WHERE id_reservations = p_reservation_id AND status = 'Активно';
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Бронь не найдена или уже не активна';
    END IF;
END;
$$ LANGUAGE plpgsql;

--для отмены брони (пластинки возвращаются в остаток)
CREATE OR REPLACE FUNCTION shem.cancel_reservation(
    p_reservation_id INTEGER
) RETURNS VOID AS $$
DECLARE
    v_record_id INTEGER;
    v_quantity INTEGER;
BEGIN
    SELECT id_record, quantity 
    INTO v_record_id, v_quantity
    FROM shem.reservations 
    WHERE id_reservations = p_reservation_id AND status = 'Активно';
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Активная бронь не найдена';
    END IF;
    
    UPDATE shem.record 
    SET remaining_quantity = remaining_quantity + v_quantity
    WHERE id_record = v_record_id;
    
    UPDATE shem.reservations 
    SET status = 'Отменено'
    WHERE id_reservations = p_reservation_id;
END;
$$ LANGUAGE plpgsql;

--для просрочивания броней
CREATE OR REPLACE FUNCTION shem.expire_old_reservations()
RETURNS INTEGER AS $$
DECLARE
    v_expired_count INTEGER;
BEGIN
    WITH expired_reservations AS (
        UPDATE shem.reservations 
        SET status = 'Просрочено'
        WHERE status = 'Активно' AND expiry_date < CURRENT_DATE
        RETURNING id_record, quantity
    )
    UPDATE shem.record r
    SET remaining_quantity = r.remaining_quantity + er.quantity
    FROM expired_reservations er
    WHERE r.id_record = er.id_record;
    
    GET DIAGNOSTICS v_expired_count = ROW_COUNT;
    RETURN v_expired_count;
END;
$$ LANGUAGE plpgsql;