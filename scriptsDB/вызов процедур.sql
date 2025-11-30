--обновление информации о CD
CALL shem.update_cd_info(
    2,
    NULL,
    NULL,
    NULL,
    499.00,
	12
);

--добавление нового CD
CALL shem.insert_new_cd(
    'DOMINO-128',
    'AM',
    '2013-09-09',
    450.00, 
    899.00, 
    15,
    'Пятый студийный альбом британской группы Arctic Monkeys. Содержит хиты "Do I Wanna Know?", "I Wanna Be Yours".'
);

--добавление нового ансамбля
CALL shem.insert_new_ensemble(
    'Arctic Monkeys',
    'Рок-группа',
    '2002-01-01'
);