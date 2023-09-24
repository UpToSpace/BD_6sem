--1.	�������� � ���������, ����� ����� �������������� ������������ ��� ���������� SQL Server.
use rent
go
--2. ������� ����������� ������� ������, ���� � �������������. ��������� ���������� ����������.
	create Login lera1 with password='lera1';		--�����
	create user lera_user for login lera1;		--����, ������. � ������
	create user myuser without login;				--����
	create role myrole;							--����

	grant select, insert, update, delete on clients to myrole;
	revoke update on clients from myrole;
	EXEC sp_addrolemember @rolename = 'myrole', @membername = 'lera_user'; -- ��������� ������������ � ������ � ����


--3.	����������������� ������������� ���� ��� ����� ��������� � ���� ������.
	create login john with password = 'john';
	create login jane with password = 'jane';
	create user john for login John;
	create user jane for login Jane;

	exec sp_addrolemember 'db_datareader', 'John';
	exec sp_addrolemember 'db_ddladmin', 'John';
	deny select on clients to Jane;

	drop procedure getclients;

	create procedure getclients 
		with execute as 'John'
		as select * from clients;

	alter authorization on getclients to John;
	grant execute on getclients to Jane;
	
	--
	setuser 'Jane';
	exec getclients;
	select * from clients;
	setuser;

---- � � � � � � � � �     � � � � � ----

--4.	������� ��� ���������� SQL Server ������ ������. 
	use master;
	go

	create server audit Audit 
	to file(
		filepath = 'D:/University/��/labs/lab10',
		maxsize = 10 mb,
		max_rollover_files = 0,
		reserve_disk_space = off
	) with ( queue_delay = 1000, on_failure = continue);

	create server audit PAudit to application_log;
	create server audit AAudit to security_log;

	--
	select * from sys.server_audits;


--5.	������ ��� ���������� ������ ����������� ������������.
	create server audit specification ServerAuditSpecification12
		for server audit Audit
		add (database_change_group)
		with (state=on)


--6.	��������� ��������� �����, ������������������ ������ ������.
	alter server audit Audit with (state=on);

	create database primer;
	drop database primer;
	go

	select statement from fn_get_audit_file('D:\University\��\labs\lab10\Audit_D88D0A42-F4C4-4384-9A56-A5E9E35CCC9E_0_133295111797670000.sqlaudit', null, null);	

---- � � � � �    � � � �    � � � � � � ----

--7.	������� ����������� ������� ������, 8.	������ ��� ������ ����������� ������������, 9.	��������� ����� ��, ������������������ ������ ������
use rent;
go
create database audit specification DBAudit
for server audit Audit 
add (insert on rent.dbo.clients by dbo)
with (state=on);

select * from rent.dbo.clients
INSERT INTO clients VALUES ('�������96','���96', 'MP000096', NULL, '������� ����');

select statement from fn_get_audit_file('D:\University\��\labs\lab10\Audit_D88D0A42-F4C4-4384-9A56-A5E9E35CCC9E_0_133295111797670000.sqlaudit', null, null);	


--10.	���������� ����� �� � �������.
use master;
go
alter server audit Audit with (state=off);
alter server audit PAudit with (state=off);
alter server audit AAudit with (state=off);


---- � � � � � � � � � � ----

--11.	������� ��� ���������� SQL Server ������������� ���� ����������.
use rent;
go
create asymmetric key Akey
with algorithm = rsa_2048
encryption by password = 'passwordddd';

--12.	����������� � ������������ ������ ��� ������ ����� �����.
declare @text nvarchar(21);
declare @ctext nvarchar (256);
set @text = 'im a simple text';
print @text;
set @ctext = EncryptByAsymKey(AsymKey_ID('AKey'), @text);
print @ctext;
set @text = DecryptByAsymKey(AsymKey_ID('AKey'), @ctext, N'passwordddd');
print @text;


--13.	������� ��� ���������� SQL Server ����������.
create certificate Cert
encryption by password = N'passwordddd'
with subject = N'Certificate',
expiry_date = N'25.05.2024'


--14.	����������� � ������������ ������ ��� ������ ����� �����������.
	declare @plain_text nvarchar(58);
	set @plain_text = 'im a simple text';
	print @plain_text;

	declare @cipher_text nvarchar(256);
	set @cipher_text = EncryptByCert(Cert_ID('Cert'), @plain_text);
	print @cipher_text;

	set @plain_text = CAST(DecryptByCert(Cert_ID('Cert'), @cipher_text, N'passwordddd') as nvarchar(58));
	print @plain_text;
	

--15.	������� ��� ���������� SQL Server ������������ ���� ���������� ������.
create symmetric key LKey
with algorithm = AES_256
encryption by password = N'passwordddd';

open symmetric key LKey 
decryption by password = N'passwordddd';


--16.	����������� � ������������ ������ ��� ������ ����� �����.
declare @text nvarchar(21);
declare @ctext nvarchar (256);
set @text = 'im a simple text';
print @text;
set @ctext = EncryptByKey(Key_GUID('LKey'), @text);
print @ctext;
set @text = CAST(DecryptByKey(@ctext) as nvarchar(21));
print @text;

close symmetric key LKey;

--17.	������������������ ���������� ���������� ���� ������.
use master;
create master key encryption by password = 'passwordddd';

create certificate MCert 
with subject = 'certificate', 
expiry_date = '25.05.2024';

use rent;
create database encryption key
with algorithm = AES_256
encryption by server certificate MCert;


--18.	������������������ ���������� �����������.
select HashBytes('MD4', 'hello');
select HashBytes('SHA1', 'hello');


--19.	������������������ ���������� ��� ��� ������ �����������.
select * from sys. certificates;
select SIGNBYCERT(Cert_ID('Cert'), N'hello', N'passwordddd') as ���;	
select VERIFYSIGNEDBYCERT(Cert_ID('Cert'), N'hello', 0x0100050204000000555351A73688E246E941C8DC470D2F47BF47D51E1CCEF3F4E58E94092466A157806E5FE494C1E6EF1CCBDA70093FFF6F468A5CE4D1345648DDD0D9C415FDE6ACD2593947559141DD459E3BD6ECA3367C3FD218389271A42C27CAED9733139D9498E6A9162D4EAA459463A41B5DEDFE559FB21CCEBEBD293B962A9BB2F59EF7C172DD0FDB711F806A531243C7E64A4B0D6397B5163B8FB83953BB8D393FACBDAE2B2D15D61812F2EAD6B88B8A56BC905B9ED3185AE275B4EF3DD93FAA9BF330437A7CBEBD629B5E653DB25768E7D88657AF24EC204DB85BB421C26F5893928E82B70D4DAFDDCA250BE1387475E8402D32CC05E2EC1E63469279A327EC9DE52EB7);

--20.	������� ��������� ����� ����������� ������ � ������������.
backup certificate Cert
to file = N'D:/University/��/labs/lab10/Cert.cer'
with private key(
file = N'D:/University/��/labs/lab10/Cert.pvk',
encryption by password = N'passwordddd',
decryption by password = N'passwordddd');

use master;
BACKUP MASTER KEY TO FILE = 'D:/University/��/labs/lab10/MasterKey.key' 
ENCRYPTION BY PASSWORD = 'passwordddd';

		