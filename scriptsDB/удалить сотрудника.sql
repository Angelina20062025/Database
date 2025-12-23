ALTER TABLE shem.employees 
ADD COLUMN IF NOT EXISTS is_deleted BOOLEAN DEFAULT false;

CREATE OR REPLACE PROCEDURE shem.archive_employee(
    p_id_employees INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE shem.employees 
    SET is_deleted = true
    WHERE id_employees = p_id_employees;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Сотрудник % не найден', p_id_employees;
    END IF;
END;
$$;