INSERT INTO shem.musicians (first_name, last_name, birth_date) VALUES
('Алекс', 'Тёрнер', '1986-01-06'),
('Джейми', 'Кук', '1985-07-08'),
('Мэтт', 'Хелдерс', '1986-09-03'),
('Ник', 'О''Мэлли', '1985-11-05');

INSERT INTO shem.ensemble_members (id_ensembles, id_musicians) 
SELECT 
    (SELECT id_ensembles FROM shem.ensembles WHERE name = 'Arctic Monkeys'),
    id_musicians 
FROM shem.musicians 
WHERE last_name IN ('Тёрнер', 'Кук', 'Хелдерс', 'О''Мэлли');

--добавляем роли музыкантам
INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles)
SELECT 
    m.id_musicians,
    (SELECT id_musician_roles FROM shem.musician_roles WHERE name = 'Вокалист')
FROM shem.musicians m WHERE m.last_name = 'Тёрнер';

INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles)
SELECT 
    m.id_musicians,
    (SELECT id_musician_roles FROM shem.musician_roles WHERE name = 'Гитарист')
FROM shem.musicians m WHERE m.last_name IN ('Тёрнер', 'Кук');

INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles)
SELECT 
    m.id_musicians,
    (SELECT id_musician_roles FROM shem.musician_roles WHERE name = 'Ударник')
FROM shem.musicians m WHERE m.last_name = 'Хелдерс';

INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles)
SELECT 
    m.id_musicians,
    (SELECT id_musician_roles FROM shem.musician_roles WHERE name = 'Бас-гитарист')
FROM shem.musicians m WHERE m.last_name = 'О''Мэлли';

INSERT INTO shem.compositions (title, id_genres, duration_seconds, year_created) VALUES
('Do I Wanna Know?', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 272, 2013),
('R U Mine?', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 201, 2012),
('One for the Road', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 203, 2013),
('Arabella', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 212, 2013),
('I Want It All', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 181, 2013),
('No. 1 Party Anthem', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 251, 2013),
('Mad Sounds', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 193, 2013),
('Fireside', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 237, 2013),
('Why''d You Only Call Me When You''re High?', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 161, 2013),
('Snap Out of It', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 193, 2013),
('Knee Socks', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 257, 2013),
('I Wanna Be Yours', (SELECT id_genres FROM shem.genres WHERE name = 'Рок'), 183, 2013);

--создаем исполнения для альбома
INSERT INTO shem.performances (id_compositions, id_ensembles, performance_date, recording_location)
SELECT 
    c.id_compositions,
    (SELECT id_ensembles FROM shem.ensembles WHERE name = 'Arctic Monkeys'),
    '2013-05-15',
    'Sage & Sound Recording Studio, Лос-Анджелес'
FROM shem.compositions c 
WHERE c.title IN (
    'Do I Wanna Know?', 'R U Mine?', 'One for the Road', 'Arabella', 
    'I Want It All', 'No. 1 Party Anthem', 'Mad Sounds', 'Fireside',
    'Why''d You Only Call Me When You''re High?', 'Snap Out of It', 
    'Knee Socks', 'I Wanna Be Yours'
);

-- Связываем альбом с исполнениями
INSERT INTO shem.record_performances (id_record, id_performances)
SELECT 
    (SELECT id_record FROM shem.record WHERE catalog_number = 'DOMINO-128'),
    p.id_performances
FROM shem.performances p
JOIN shem.compositions c ON p.id_compositions = c.id_compositions
WHERE c.title IN (
    'Do I Wanna Know?', 'R U Mine?', 'One for the Road', 'Arabella', 
    'I Want It All', 'No. 1 Party Anthem', 'Mad Sounds', 'Fireside',
    'Why''d You Only Call Me When You''re High?', 'Snap Out of It', 
    'Knee Socks', 'I Wanna Be Yours'
);