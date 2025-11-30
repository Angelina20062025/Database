--для изменения данных о компакт-дисках
CREATE OR REPLACE PROCEDURE shem.update_cd_info(
    p_id_record INTEGER,
    p_title VARCHAR(200) DEFAULT NULL,
    p_release_date DATE DEFAULT NULL,
    p_wholesale_price NUMERIC(10,2) DEFAULT NULL,
    p_retail_price NUMERIC(10,2) DEFAULT NULL,
    p_remaining_quantity INTEGER DEFAULT NULL,
    p_description TEXT DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE shem.record 
    SET 
        title = COALESCE(p_title, title),
        release_date = COALESCE(p_release_date, release_date),
        wholesale_price = COALESCE(p_wholesale_price, wholesale_price),
        retail_price = COALESCE(p_retail_price, retail_price),
        remaining_quantity = COALESCE(p_remaining_quantity, remaining_quantity),
        description = COALESCE(p_description, description)
    WHERE id_record = p_id_record;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Компакт-диск с ID % не найден', p_id_record;
    END IF;
END;
$$;

--для ввода новых компакт-дисков
CREATE OR REPLACE PROCEDURE shem.insert_new_cd(
    p_catalog_number VARCHAR(50),
    p_title VARCHAR(200),
    p_release_date DATE,
    p_wholesale_price NUMERIC(10,2),
    p_retail_price NUMERIC(10,2),
    p_remaining_quantity INTEGER DEFAULT 0,
    p_description TEXT DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO shem.record (
        catalog_number, title, release_date, wholesale_price, 
        retail_price, remaining_quantity, description
    ) VALUES (
        p_catalog_number, p_title, p_release_date, p_wholesale_price,
        p_retail_price, p_remaining_quantity, p_description
    );
END;
$$;

--для ввода новых данных об ансамблях
CREATE OR REPLACE PROCEDURE shem.insert_new_ensemble(
    p_name VARCHAR(100),
    p_ensemble_type_name VARCHAR(50),
    p_founded_date DATE,
    p_description TEXT DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_ensemble_type_id INTEGER;
BEGIN
    SELECT id_ensemble_types INTO v_ensemble_type_id
    FROM shem.ensemble_types
    WHERE name = p_ensemble_type_name;
    
    IF v_ensemble_type_id IS NULL THEN
        RAISE EXCEPTION 'Тип ансамбля "%" не найден', p_ensemble_type_name;
    END IF;
    
    INSERT INTO shem.ensembles (
        name, id_ensemble_types, founded_date, description
    ) VALUES (
        p_name, v_ensemble_type_id, p_founded_date, p_description
    );
END;
$$;