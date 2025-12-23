ALTER TABLE shem.reservations 
ADD CONSTRAINT chk_reservation_max_duration 
CHECK (expiry_date - reservation_date <= 7);

ALTER TABLE shem.purchases 
ADD CONSTRAINT chk_purchases_purchase_date_past 
CHECK (purchase_date <= CURRENT_DATE);

ALTER TABLE shem.purchase_details 
ADD CONSTRAINT chk_purchase_details_quantity_positive 
CHECK (quantity > 0),

ADD CONSTRAINT chk_purchase_details_unit_price_positive 
CHECK (unit_price >= 0);

ALTER TABLE shem.compositions 
ADD CONSTRAINT chk_compositions_duration_positive 
CHECK (duration_seconds > 0);

ALTER TABLE shem.employees 
ADD CONSTRAINT chk_employees_phone_format 
CHECK (phone IS NULL OR phone ~ '^(\+7|8)[0-9]{10}$')

ALTER TABLE shem.customers 
ADD CONSTRAINT chk_customers_phone_format 
CHECK (phone IS NULL OR phone ~ '^(\+7|8)[0-9]{10}$')

ALTER TABLE shem.customers
ADD CONSTRAINT chk_customers_email_format 
CHECK (email IS NULL OR email ~ '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$');

ALTER TABLE shem.musicians 
ADD CONSTRAINT chk_musicians_birth_date_valid 
CHECK (birth_date IS NULL OR birth_date >= '1300-01-01' AND birth_date <= CURRENT_DATE);

ALTER TABLE shem.ensembles 
ADD CONSTRAINT chk_ensembles_founded_date_valid 
CHECK (founded_date IS NULL OR founded_date >= '1300-01-01' AND founded_date <= CURRENT_DATE);

ALTER TABLE shem.performances 
ADD CONSTRAINT chk_performances_date_valid 
CHECK (performance_date IS NULL OR performance_date >= '1300-01-01' AND performance_date <= CURRENT_DATE);

ALTER TABLE shem.reservations 
ADD CONSTRAINT chk_reservations_date_past 
CHECK (reservation_date <= CURRENT_DATE);

ALTER TABLE shem.record 
ADD CONSTRAINT chk_record_wholesale_price_positive 
CHECK (wholesale_price >= 0),

ADD CONSTRAINT chk_record_retail_price_positive 
CHECK (retail_price >= 0),

ADD CONSTRAINT chk_record_retail_gte_wholesale 
CHECK (retail_price >= wholesale_price),

ADD CONSTRAINT chk_record_sales_non_negative 
CHECK (remaining_quantity >= 0),

ADD CONSTRAINT chk_record_release_date_past 
CHECK (release_date <= CURRENT_DATE);