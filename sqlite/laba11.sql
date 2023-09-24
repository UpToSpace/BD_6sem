--2.	Создать базу данных SQLite, дополняющую вашу базу данных SQL Server по варианту.
create table units(
    id int primary key ,
    name varchar(255) not null
);

drop table units;

create table workers(
    id int primary key,
    unit_id int not null,
    full_name varchar(255) not null,
    foreign key (unit_id) references units(id) on delete cascade
);
drop table workers;

--3.	Добавить данные в базу данных SQLite.
insert into units values (1, 'western');
insert into workers values (1, 1, 'Григорий Гриша');

--4.	Создать представление в базе данных SQLite. 

create view units_workers as
select units.name, workers.full_name
from units
inner join workers on units.id = workers.unit_id;

drop view units_workers;

select * from units_workers;

--5.	Создать триггер в базе данных SQLite.

create trigger workers_trigger
after insert on workers
begin
    update workers set full_name = full_name || ' сотрудник' where id = new.id;
end;

drop trigger workers_trigger;
delete from workers where id = 2;

insert into workers values (2, 1, 'Александр Саша');
select * from workers;