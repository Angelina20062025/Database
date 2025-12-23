INSERT INTO shem.users (login, password_hash, id_employees)
VALUES ('elenasmirnova', crypt('sjkdksv1988', gen_salt('bf')), 1);

INSERT INTO shem.users (login, password_hash, id_employees)
VALUES ('alexvasilyev', crypt('jojoprod2000', gen_salt('bf')), 2);

INSERT INTO shem.users (login, password_hash, id_employees)
VALUES ('olgapopova', crypt('hadfop7659', gen_salt('bf')), 3);

select * from shem.employees
select * from shem.users