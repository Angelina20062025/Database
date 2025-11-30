-- Ансамбль существует и есть CD
SELECT * FROM shem.get_ensemble_cds('The Beatles');

-- Ансамбль не существует
SELECT * FROM shem.get_ensemble_cds('Несуществующий ансамбль');

-- Ансамбль существует, но нет CD
SELECT * FROM shem.get_ensemble_cds('Тест');