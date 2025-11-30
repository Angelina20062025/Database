INSERT INTO shem.reservations (id_customers, id_employees, id_record, reservation_date, status, quantity, expiry_date)
VALUES (2, 2, 1, '2024-03-15', 'Отменено', 1, '2024-03-22');

INSERT INTO shem.purchases (id_customers, id_employees, purchase_date, id_payment_methods)
VALUES (2, 3, '2024-10-08', 2);

INSERT INTO shem.purchase_details (id_purchases, id_record, quantity, unit_price)
VALUES (2, 3, 1, 799.00);

select * from shem.purchases
select * from shem.purchase_details
select * from shem.payment_methods
select * from shem.record
select * from shem.customers
select * from shem.reservation_statuses
select * from shem.reservations
select * from shem.musicians