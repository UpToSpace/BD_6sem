use master;
create database rent;
go
use rent;

create table clients(
surname nvarchar(50), 
name nvarchar(50),
passport_number nvarchar(50) primary key
)

create table apartments(
apartment_id int identity(1, 1) primary key,
city nvarchar(50), 
street nvarchar(50),
house_number int,
room_number int,
day_cost money
)

create table rents(
rent_id int identity(1, 1) primary key,
apartment_id int foreign key references apartments(apartment_id),
status nvarchar(50),
date_begin date,
date_end date,
client_passport_number nvarchar(50) foreign key references clients(passport_number))

create table bill_details(
bill_id int identity(1, 1) primary key,
rent_id int foreign key references rents(rent_id),
bill_date date,
total money)

insert into clients values ('Фамилия1', 'Имя1', 'MP000001')
insert into clients values ('Фамилия2', 'Имя2', 'MP000002')
insert into clients values ('Фамилия3', 'Имя3', 'MP000003')
insert into clients values ('Фамилия4', 'Имя4', 'MP000004')
insert into clients values ('Фамилия5', 'Имя5', 'MP000005')

select * from clients;

insert into apartments values ('Минск', 'Ленинградская', 9, 130, 265);
insert into apartments values ('Барановичи', 'Восточная', 4, 20, 95);
insert into apartments values ('Бобруйск', 'Древняя', 11, 2, 89);
insert into apartments values ('Солигорск', 'Криминальная', 32, 8, 107);
insert into apartments values ('Гродно', 'Пешеходная', 11, 1, 260);

select * from apartments;

select (sum(day_cost) - min(day_cost) - max(day_cost)) / (count(day_cost) - 2) from apartments

insert into rents values (7, 'not paid', '2023-02-16', '2023-02-18', 'MP000001')
insert into rents values (8, 'not paid', '2023-02-12', '2023-02-13', 'MP000002')
insert into rents values (9, 'not paid', '2023-02-11', '2023-02-17', 'MP000003')
insert into rents values (10, 'not paid', '2023-02-08', '2023-02-15', 'MP000004')
insert into rents values (11, 'not paid', '2023-02-04', '2023-02-14', 'MP000005')

update rents set type = 'Краткосрочная аренда' where rent_id = 17 and rent_id = 14
update rents set type = 'Аренда комнаты' where rent_id = 13 and rent_id = 15 and rent_id = 16
update rents set type = 'Субаренда' where rent_id = 12 and rent_id = 19

select * from rents;

delete from bill_details;
delete from rents;
delete from apartments;
delete from clients;


-- drop function rent_days_amount;
-- ф выводит количество дней аренда
create function rent_days_amount (@rent_id int)
returns int as
begin
declare @date_begin date, @date_end date;
set @date_begin = (select date_begin from rents where rent_id = @rent_id);
set @date_end = (select date_end from rents where rent_id = @rent_id);
return DATEDIFF(day, @date_begin, @date_end);
end;

print dbo.rent_days_amount(12);
print dbo.rent_days_amount(13);

-- drop view rents_with_apartments
create view rents_with_apartments 
as
select client_passport_number, rent_id, city, street, house_number, room_number, day_cost, date_begin, date_end from rents inner join apartments 
on rents.apartment_id = apartments.apartment_id;

select * from rents_with_apartments;

--триггер при вставке в билл меняет статус рента
-- drop trigger bill_insert
create trigger bill_insert 
on bill_details
after insert
as
declare @rent_id int;
set @rent_id = (select rent_id from inserted);
update rents set status = 'paid' from rents inner join inserted on rents.rent_id = inserted.rent_id;

select * from bill_details;
insert into bill_details values (17, '2023-03-09', 530)
select * from rents;

-- drop trigger bill_delete
create trigger bill_delete
on bill_details
after delete
as 
update rents set status = 'not paid' from rents
inner join deleted on rents.rent_id = deleted.rent_id

delete bill_details where rent_id = 17
select * from rents;

create trigger rents_delete
on rents
after delete
as
print 'rent was deleted';

delete from rents where apartment_id = 9;
select * from rents

insert into rents values (9, 'not paid', '2023-03-03', '2023-03-05', 'MP000004', 'Субаренда')

-- drop procedure pay_for_apartment 
-- delete dbo.bill_details

create procedure pay_for_apartment @rent_id int
as
begin
if (not exists(select * from bill_details where rent_id = @rent_id))
	begin
		declare @total money, @per_day money, @days int, @date date;
		set @days = dbo.rent_days_amount(@rent_id);
		set @per_day = (select top 1 day_cost from apartments inner join rents on apartments.apartment_id = rents.apartment_id
		where rents.rent_id = @rent_id);
		set @total = @days * @per_day;
		set @date = CONVERT(date, GETDATE());
		insert into bill_details values (@rent_id, @date, @total);
	end;
end;

exec pay_for_apartment 17;
exec pay_for_apartment 13;

select * from rents;
select * from bill_details;

create view clients_with_bills
as
select bill_date, total, status, surname, name, passport_number
from bill_details left join rents 
on bill_details.rent_id = rents.rent_id
left join clients on clients.passport_number = rents.client_passport_number;

select * from clients_with_bills;

create view bills_with_apartments
as
select bill_id, bill_date, total, city, street, house_number, room_number
from bill_details left join rents
on bill_details.rent_id = rents.rent_id
left join apartments on apartments.apartment_id = rents.apartment_id;

select * from bills_with_apartments;

create procedure register_client @surname nvarchar(50), @name nvarchar(50), @passport_number nvarchar(50)
as
begin
	if (not exists(select * from clients where passport_number = @passport_number))
	begin
	insert into clients values (@surname, @name, @passport_number);
	end;
end;

exec register_client 'Фамилия21', 'Имя21', 'MP000021';

select * from clients;

delete from clients where passport_number = 'MP000021'

create procedure register_apartment @city nvarchar(50), @street nvarchar(50), @house_number int, @room_number int, @day_cost money
as
	begin
	if (not exists(select * from apartments where @city = city and @street = street and @house_number = house_number and @room_number = room_number))
	begin
	insert into apartments values(@city, @street, @house_number, @room_number, @day_cost);
	end;
end;

exec register_apartment 'Смолевичи', 'Доброномская', 10, 1, 95 

select * from apartments;

-- drop procedure rent_apartment
create procedure rent_apartment @apartment_id int, @date_begin date, @date_end date, @client_passport_number nvarchar(50), @type nvarchar(50)
as
begin
if (datediff(day, @date_end, @date_begin) < 0)
begin
	if(not exists(select * from rents where date_begin < @date_end and
	date_end > @date_begin and apartment_id = @apartment_id))
	begin
	insert into rents values(@apartment_id, 'not paid', @date_begin, @date_end, @client_passport_number, @type);
	end;
	else
	begin
	print('cannot rent');
	end;
	end;
end;

exec rent_apartment 9, '2023-03-08', '2023-03-09', 'MP000004';
exec rent_apartment 10, '2023-03-03', '2023-03-05', 'MP000004';

select * from rents;

select * from apartments; 

-- ф выводит колво аренд у клиента
create function client_rent_amount (@client_passport_number nvarchar(50))
returns int as
begin
declare @result int;
set @result = (select count(*) from rents  
left join bill_details 
on bill_details.rent_id = rents.rent_id
where @client_passport_number = client_passport_number)
return @result;
end;

print dbo.client_rent_amount('MP000001');
print dbo.client_rent_amount('MP000020');

-- ф выводит заработок
create function total_money()
returns money as
begin
declare @result money;
set @result = (select sum(total) from bill_details);
return @result;
end;

print dbo.total_money();
---


create nonclustered index rent_index on
dbo.bill_details(bill_date) include (rent_id);

drop index rent_index on bill_details

select rent_id from bill_details where bill_date = '2023-02-16';


create nonclustered index date_index on
dbo.rents(date_begin) where date_begin > '2023-02-02' and date_begin < '2023-02-20';

drop index date_index on rents;

select date_begin from rents where date_begin > '2023-02-08' and date_begin < '2023-02-18';



create nonclustered index apartment_index on
dbo.apartments(city) include (street, house_number, room_number);

select city from apartments where city = 'Минск' and street = 'Ленинградская';




