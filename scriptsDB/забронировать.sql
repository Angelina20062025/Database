--бронь на 7 дней покупатель сотрудник пластинка количество
SELECT * FROM shem.create_reservation(2, 2, 3, 1, 7, 'Предоплата внесена');

SELECT * FROM shem.create_reservation(3, 2, 5, 1, 7);