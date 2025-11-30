CREATE TABLE shem.genres (
    id_genres SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE shem.payment_methods (
    id_payment_methods SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE shem.employee_roles (
    id_employee_roles SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE shem.ensemble_types (
    id_ensemble_types SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE shem.musician_roles (
    id_musician_roles SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Основные таблицы
CREATE TABLE shem.users (
    id_users SERIAL PRIMARY KEY,
    login VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL
);

CREATE TABLE shem.employees (
    id_employees SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    patronymic VARCHAR(100),
    phone VARCHAR(20),
    id_employee_roles INTEGER REFERENCES shem.employee_roles(id_employee_roles),
    id_users INTEGER UNIQUE REFERENCES shem.users(id_users) ON DELETE SET NULL
);

CREATE TABLE shem.customers (
    id_customers SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    patronymic VARCHAR(100),
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(100)
);

CREATE TABLE shem.musicians (
    id_musicians SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    patronymic VARCHAR(100),
    birth_date DATE,
    id_musician_roles INTEGER REFERENCES shem.musician_roles(id_musician_roles),
    bio TEXT,
    photo BYTEA
);

CREATE TABLE shem.ensembles (
    id_ensembles SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    id_ensemble_types INTEGER REFERENCES shem.ensemble_types(id_ensemble_types),
    founded_date DATE,
    description TEXT,
    photo BYTEA
);

CREATE TABLE shem.ensemble_members (
    id_ensembles INTEGER NOT NULL REFERENCES shem.ensembles(id_ensembles) ON DELETE CASCADE,
    id_musicians INTEGER NOT NULL REFERENCES shem.musicians(id_musicians) ON DELETE CASCADE,
    PRIMARY KEY (id_ensembles, id_musicians)
);

CREATE TABLE shem.compositions (
    id_compositions SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    id_genres INTEGER REFERENCES shem.genres(id_genres),
    duration_seconds INTEGER NOT NULL,
    year_created INTEGER
);

CREATE TABLE shem.performances (
    id_performances SERIAL PRIMARY KEY,
    id_compositions INTEGER NOT NULL REFERENCES shem.compositions(id_compositions),
    id_ensembles INTEGER REFERENCES shem.ensembles(id_ensembles),
    performance_date DATE,
    recording_location VARCHAR(100)
);

CREATE TABLE shem.record (
    id_record SERIAL PRIMARY KEY,
    catalog_number VARCHAR(50) NOT NULL,
    title VARCHAR(200) NOT NULL,
    release_date DATE NOT NULL,
    wholesale_price DECIMAL(10,2) NOT NULL,
    retail_price DECIMAL(10,2) NOT NULL,
    last_year_sales INTEGER DEFAULT 0,
    current_year_sales INTEGER DEFAULT 0,
    remaining_quantity INTEGER DEFAULT 0,
    cover_image BYTEA,
    description TEXT
);

CREATE TABLE shem.record_performances (
    id_record INTEGER NOT NULL REFERENCES shem.record(id_record) ON DELETE CASCADE,
    id_performances INTEGER NOT NULL REFERENCES shem.performances(id_performances) ON DELETE CASCADE,
    PRIMARY KEY (id_record, id_performances)
);

CREATE TABLE shem.reservations (
    id_reservations SERIAL PRIMARY KEY,
    id_customers INTEGER NOT NULL REFERENCES shem.customers(id_customers),
    id_record INTEGER NOT NULL REFERENCES shem.record(id_record),
    reservation_date TIMESTAMP DEFAULT NOW(),
    status VARCHAR(20) DEFAULT 'active'
);

CREATE TABLE shem.purchases (
    id_purchases SERIAL PRIMARY KEY,
    id_customers INTEGER NOT NULL REFERENCES shem.customers(id_customers),
    id_employees INTEGER NOT NULL REFERENCES shem.employees(id_employees),
    purchase_date TIMESTAMP DEFAULT NOW(),
    total_amount DECIMAL(10,2) NOT NULL,
    id_payment_methods INTEGER REFERENCES shem.payment_methods(id_payment_methods)
);

CREATE TABLE shem.purchase_details (
    id_purchase_details SERIAL PRIMARY KEY,
    id_purchases INTEGER NOT NULL REFERENCES shem.purchases(id_purchases) ON DELETE CASCADE,
    id_record INTEGER NOT NULL REFERENCES shem.record(id_record),
    quantity INTEGER NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL
);

CREATE TABLE shem.record_sales (
    id_record_sales SERIAL PRIMARY KEY,
    id_record INTEGER NOT NULL REFERENCES shem.record(id_record),
    sale_date DATE NOT NULL,
    quantity INTEGER NOT NULL,
    year INTEGER GENERATED ALWAYS AS (EXTRACT(YEAR FROM sale_date)) STORED
);

INSERT INTO shem.genres (name) VALUES 
('Классическая музыка'),
('Джаз'),
('Рок'),
('Поп-музыка'),
('Электронная музыка'),
('Хип-хоп'),
('Фолк'),
('Блюз'),
('Кантри'),
('Рэп'),
('Метал'),
('Шансон'),
('Диско'),
('Регги'),
('Соул');

INSERT INTO shem.payment_methods (name) VALUES 
('Наличные'),
('Банковская карта'),
('Электронный кошелек'),
('Перевод');

INSERT INTO shem.employee_roles (name) VALUES 
('Администратор'),
('Менеджер'),
('Продавец'),
('Консультант'),
('Старший продавец');

INSERT INTO shem.ensemble_types (name) VALUES 
('Оркестр'),
('Джаз-бэнд'),
('Квартет'),
('Квинтет'),
('Рок-группа'),
('Хор'),
('Симфонический оркестр'),
('Камерный ансамбль'),
('Биг-бэнд'),
('Септет'),
('Октет'),
('Трио'),
('Дуэт');

INSERT INTO shem.musician_roles (name) VALUES 
('Исполнитель'),
('Композитор'),
('Дирижер'),
('Руководитель ансамбля'),
('Аранжировщик'),
('Вокалист'),
('Бэк-вокалист'),
('Саунд-продюсер');