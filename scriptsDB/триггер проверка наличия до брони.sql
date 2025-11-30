CREATE OR REPLACE FUNCTION shem.check_availability_before_reservation()
RETURNS TRIGGER AS $$
DECLARE
    v_remaining_quantity INTEGER;
BEGIN
    SELECT remaining_quantity INTO v_remaining_quantity
    FROM shem.record WHERE id_record = NEW.id_record;
    
    IF v_remaining_quantity < NEW.quantity THEN
        RAISE EXCEPTION 'Недостаточно пластинок для брони. Доступно: %, Запрошено: %', 
                        v_remaining_quantity, NEW.quantity;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;