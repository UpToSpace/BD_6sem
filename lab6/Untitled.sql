alter session set "_ORACLE_SCRIPT"=TRUE;
grant all privileges to migr1 identified by 1111;
delete rents;
select * from clients;
select * from bill_details;
select * from rents;
select * from apartments;
drop table rents CASCADE CONSTRAINTS;
SELECT * FROM USER_CONSTRAINTS WHERE TABLE_NAME = 'rents'
alter table rents drop constraint rents_fk;
SELECT * FROM rents WHERE DATE_BEGIN > to_date('2023-02-03', 'yyyy-mm-dd') AND DATE_END < to_date('2023-02-19', 'yyyy-mm-dd') ORDER BY type
SELECT * FROM rents WHERE DATE_BEGIN > '2023-02-04' AND DATE_END < '2023-02-18' ORDER BY type

create table rents(
rent_id number(10) GENERATED AS IDENTITY (START WITH 1 INCREMENT BY 1 NOCYCLE ORDER),
apartment_id number(10),
status varchar2(50),
date_begin date,
date_end date,
client_passport_number varchar2(50),
type varchar2(50),
    CONSTRAINT rents_pk PRIMARY KEY (rent_id),
    constraint apartment_fk foreign key (apartment_id) references apartments(apartment_id),
    constraint clients_fk foreign key (client_passport_number) references clients(passport_number)
    )

CREATE TABLE apartments (
    apartment_id   NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    city           VARCHAR2(50),
    street         VARCHAR2(50),
    house_number   NUMBER,
    room_number    NUMBER,
    day_cost       NUMBER(10,2),
    location       SDO_GEOMETRY
);

CREATE TABLE clients (
    surname VARCHAR2(50),
    name VARCHAR2(50),
    passport_number VARCHAR2(50) NOT NULL PRIMARY KEY
);
alter table rents add constraint rents_fk foreign key (apartment_id) references apartments(apartment_id);
ALTER TABLE clients ADD hid VARCHAR2(4000);

INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия1', N'Имя1', N'MP000001', '0x58');
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия2', N'Имя2', N'MP000002', '0x5BC0');
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия3', N'Имя3', N'MP000003', '0x5AC0');
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия4', N'Имя4', N'MP000004', '0x5BD6');
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия5', N'Имя5', N'MP000005', '0x5BDA');
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия12', N'Имя12', N'MP000012', null);
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия13', N'Имя13', N'MP000013', '0x5B40');
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'Фамилия20', N'Имя20', N'MP000020', null);
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'l', N'l', N'MP000021', null);
INSERT INTO clients (surname, name, passport_number, hid) VALUES (N'hh', N'jj', N'pass', null);

insert into apartments(city, street, house_number, room_number, day_cost, location) values ('Минск', 'Ленинградская', 9, 130, 265, null);
insert into apartments(city, street, house_number, room_number, day_cost, location) values ('Барановичи', 'Восточная', 4, 20, 95, null);
insert into apartments(city, street, house_number, room_number, day_cost, location) values ('Бобруйск', 'Древняя', 11, 2, 89, null);
insert into apartments(city, street, house_number, room_number, day_cost, location) values ('Солигорск', 'Криминальная', 32, 8, 107, null);
insert into apartments(city, street, house_number, room_number, day_cost, location) values ('Гродно', 'Пешеходная', 11, 1, 260, null);

insert into rents(apartment_id, status, date_begin, date_end, client_passport_number, type) values (1, 'not paid', TO_DATE('2023-02-16','YYYY-MM-DD'), TO_DATE('2023-02-18','YYYY-MM-DD'), 'MP000001', 'комната');
insert into rents(apartment_id, status, date_begin, date_end, client_passport_number, type) values (2, 'not paid', TO_DATE('2023-02-12','YYYY-MM-DD'), TO_DATE('2023-02-13','YYYY-MM-DD'), 'MP000002', 'дом');
insert into rents(apartment_id, status, date_begin, date_end, client_passport_number, type) values (3, 'not paid', TO_DATE('2023-02-11','YYYY-MM-DD'), TO_DATE('2023-02-17','YYYY-MM-DD'), 'MP000003', 'пол дома');
insert into rents(apartment_id, status, date_begin, date_end, client_passport_number, type) values (4, 'not paid', TO_DATE('2023-02-08','YYYY-MM-DD'), TO_DATE('2023-02-15','YYYY-MM-DD'), 'MP000004', 'дом');
insert into rents(apartment_id, status, date_begin, date_end, client_passport_number, type) values (5, 'not paid', TO_DATE('2023-02-04','YYYY-MM-DD'), TO_DATE('2023-02-14','YYYY-MM-DD'), 'MP000005', 'комната');
CREATE OR REPLACE TRIGGER bill_insert
AFTER INSERT ON bill_details
FOR EACH ROW
BEGIN
  UPDATE rents
  SET status = 'paid'
  WHERE rent_id = :new.rent_id;
END;
CREATE OR REPLACE TRIGGER bill_delete
AFTER DELETE ON bill_details
FOR EACH ROW
BEGIN
    UPDATE rents
    SET status = 'not paid'
    WHERE rent_id = :old.rent_id;
END;