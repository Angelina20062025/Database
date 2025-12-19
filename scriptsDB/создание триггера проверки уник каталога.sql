CREATE OR REPLACE FUNCTION shem.check_catalog_number_unique()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM shem.record 
        WHERE catalog_number = NEW.catalog_number 
        AND id_record != COALESCE(NEW.id_record, 0)
    ) THEN
        RAISE EXCEPTION 
            'Каталог "%" уже существует. Пожалуйста, выберите другое название.', 
            NEW.catalog_number;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER catalog_number_unique_trigger
BEFORE INSERT OR UPDATE ON shem.record
FOR EACH ROW
EXECUTE FUNCTION shem.check_catalog_number_unique();