CREATE OR REPLACE VIEW shem.reservations_view AS
SELECT 
    r.id_reservations,
    c.first_name || ' ' || c.last_name as customer_name,
    e.first_name || ' ' || e.last_name as employee_name,
    rec.title as record_title,
    r.quantity,
    r.reservation_date,
    r.expiry_date,
    rs.status_name as status,
    r.notes,
    CASE 
        WHEN r.expiry_date < CURRENT_DATE AND r.status = 'Активно' THEN 'СРОК ИСТЕК'
        WHEN r.expiry_date = CURRENT_DATE AND r.status = 'Активно' THEN 'ИСТЕКАЕТ СЕГОДНЯ'
        ELSE 'НОРМА'
    END as urgency
FROM shem.reservations r
JOIN shem.customers c ON r.id_customers = c.id_customers
JOIN shem.employees e ON r.id_employees = e.id_employees
JOIN shem.record rec ON r.id_record = rec.id_record
JOIN shem.reservation_statuses rs ON r.status = rs.status_name;