CREATE TABLE shem.reservation_statuses (
    id_status SERIAL PRIMARY KEY,
    status_name TEXT NOT NULL UNIQUE
);

INSERT INTO shem.reservation_statuses (status_name) VALUES 
('Активная бронь'),
('Бронь завершена (покупка состоялась)'),
('Бронь просрочена'),
('Бронь отменена');