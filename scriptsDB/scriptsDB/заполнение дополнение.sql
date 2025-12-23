INSERT INTO shem.musicians (first_name, last_name, birth_date) VALUES
('Джордж', 'Харрисон', '1943-02-25'),
('Ринго', 'Старр', '1940-07-07');

INSERT INTO shem.musicians (first_name, last_name, birth_date) VALUES
('Брайан', 'Мэй', '1947-07-19'),
('Роджер', 'Тейлор', '1949-07-26'),
('Джон', 'Дикон', '1951-08-19');

INSERT INTO shem.musicians (first_name, last_name, patronymic, birth_date) VALUES
('Герберт', 'фон Караян', NULL, '1908-04-05'),
('Леонард', 'Бернстайн', NULL, '1918-08-25');

INSERT INTO shem.ensemble_members (id_ensembles, id_musicians) VALUES
(2, 15),
(2, 16);

INSERT INTO shem.ensemble_members (id_ensembles, id_musicians) VALUES
(3, 17),
(3, 18),
(3, 19);

INSERT INTO shem.ensemble_members (id_ensembles, id_musicians) VALUES
(1, 20),
(1, 21);

INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles) VALUES
(15, 6), -- Джордж Харрисон - Вокалист
(15, 8), -- Джордж Харрисон - Гитарист
(16, 6), -- Ринго Старр - Вокалист
(16, 1); -- Ринго Старр - Барабанщик

-- Для Queen
INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles) VALUES
(17, 6), -- Брайан Мэй - Вокалист
(17, 8), -- Брайан Мэй - Гитарист
(18, 6), -- Роджер Тейлор - Вокалист
(18, 1), -- Роджер Тейлор - Барабанщик
(19, 6), -- Джон Дикон - Вокалист
(19, 10); -- Джон Дикон - Бас-гитарист

-- Для классических музыкантов
INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles) VALUES
(7, 7),  -- Плетнев - Пианист
(20, 3), -- Караян - Дирижер
(21, 3); -- Бернстайн - Дирижер

INSERT INTO shem.compositions (title, id_genres, duration_seconds, year_created) VALUES
-- Классические произведения
('Реквием', 1, 3600, 1791),
('Симфония №5', 1, 2100, 1808),
('Щелкунчик', 1, 5400, 1892),
('Спящая красавица', 1, 4800, 1890),

-- The Beatles
('Let It Be', 4, 243, 1970),
('Hey Jude', 4, 431, 1968),
('Come Together', 4, 259, 1969),

-- Queen
('We Will Rock You', 3, 122, 1977),
('Another One Bites the Dust', 3, 216, 1980),
('We Are the Champions', 3, 179, 1977);

INSERT INTO shem.performances (id_compositions, id_ensembles, performance_date, recording_location) VALUES
-- Венский оркестр
(20, 1, '2021-11-10', 'Венская государственная опера'),  -- Реквием
(21, 1, '2022-02-15', 'Золотой зал Вены'),              -- Симфония №5

-- The Beatles
(24, 2, '1970-01-31', 'Apple Studios'),                -- Let It Be
(25, 2, '1968-08-30', 'Trident Studios'),              -- Hey Jude
(26, 2, '1969-07-21', 'Abbey Road Studios'),           -- Come Together

-- Queen
(27, 3, '1977-10-07', 'Mountain Studios'),             -- We Will Rock You
(28, 3, '1980-06-30', 'Musicland Studios'),            -- Another One Bites the Dust
(29, 3, '1977-10-07', 'Mountain Studios'),             -- We Are the Champions

-- БСО (Большой симфонический оркестр)
(22, 5, '2020-12-15', 'Концертный зал Чайковского'),   -- Щелкунчик
(7, 5, '2018-03-20', 'Большой театр');                 -- Кармен-сюита

INSERT INTO shem.record (catalog_number, title, release_date, wholesale_price, retail_price, remaining_quantity, description) VALUES
('DG-234', 'Вивальди: Времена года', '2022-03-20', 280.00, 549.00, 20, 'Исполнение квартета имени Бородина. Великолепная акустика Московской консерватории.');

INSERT INTO shem.record (catalog_number, title, release_date, wholesale_price, retail_price, remaining_quantity, description) VALUES
('DECCA-002', 'Моцарт: Реквием', '2022-09-10', 320.00, 649.00, 15, 'Трагическое и возвышенное произведение в исполнении Венского оркестра под управлением фон Караяна'),
('APPLE-017', 'The Beatles: Let It Be', '1970-05-08', 180.00, 399.00, 8, 'Последний студийный альбом The Beatles, записанный на крыше Apple Corps'),
('EMI-178', 'Queen: A Night at the Opera', '1975-11-21', 420.00, 849.00, 12, 'Культовый альбом Queen с Bohemian Rhapsody'),
('MELODIYA-599', 'Чайковский: Щелкунчик', '2021-12-01', 370.00, 749.00, 18, 'Полная версия балета в исполнении БСО под управлением Плетнева'),
('DG-245', 'Бетховен: Симфония №5', '2023-04-15', 310.00, 629.00, 22, 'Легендарная симфония в исполнении Венского филармонического оркестра');

-- Вивальди: Времена года
INSERT INTO shem.record_performances (id_record, id_performances) 
SELECT (SELECT id_record FROM shem.record WHERE catalog_number = 'DG-234'), id_performances
FROM shem.performances 
WHERE id_compositions = 6; -- Времена года

-- Моцарт: Реквием
INSERT INTO shem.record_performances (id_record, id_performances) 
SELECT (SELECT id_record FROM shem.record WHERE catalog_number = 'DECCA-002'), id_performances
FROM shem.performances 
WHERE id_compositions = 20; -- Реквием

-- The Beatles: Let It Be
INSERT INTO shem.record_performances (id_record, id_performances) 
SELECT (SELECT id_record FROM shem.record WHERE catalog_number = 'APPLE-017'), id_performances
FROM shem.performances 
WHERE id_compositions = 24; -- Let It Be

-- Queen: A Night at the Opera
INSERT INTO shem.record_performances (id_record, id_performances) VALUES
((SELECT id_record FROM shem.record WHERE catalog_number = 'EMI-178'), 4),  -- Bohemian Rhapsody
((SELECT id_record FROM shem.record WHERE catalog_number = 'EMI-178'), 24), -- We Will Rock You
((SELECT id_record FROM shem.record WHERE catalog_number = 'EMI-178'), 26); -- We Are the Champions

-- Чайковский: Щелкунчик
INSERT INTO shem.record_performances (id_record, id_performances) 
SELECT (SELECT id_record FROM shem.record WHERE catalog_number = 'MELODIYA-599'), id_performances
FROM shem.performances 
WHERE id_compositions = 22; -- Щелкунчик

-- Бетховен: Симфония №5
INSERT INTO shem.record_performances (id_record, id_performances) 
SELECT (SELECT id_record FROM shem.record WHERE catalog_number = 'DG-245'), id_performances
FROM shem.performances 
WHERE id_compositions = 21; -- Симфония №5