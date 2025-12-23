-- Вычисление общей суммы покупки
SELECT 
    p.id_purchases,
    p.purchase_date,
    SUM(pd.quantity * pd.unit_price) as total_amount
FROM shem.purchases p
JOIN shem.purchase_details pd ON p.id_purchases = pd.id_purchases
GROUP BY p.id_purchases, p.purchase_date;