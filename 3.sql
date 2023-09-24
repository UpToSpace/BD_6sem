exec sp_configure 'clr enabled', 1;
reconfigure;

exec sp_configure 'show advanced options', 1;
reconfigure;

exec sp_configure 'clr strict security', 0;
reconfigure;
go

ALTER DATABASE rent SET TRUSTWORTHY ON


drop function SqlStoredProcedure1;
drop type passport;
drop assembly MyAssembly;

------ assembly -----------
create assembly MyAssembly
	from 'D:\University\бд\labs\lab3\lab3\bin\Debug\lab3.dll'
	with permission_set = safe;
go

------stored procedure----------
create function SqlStoredProcedure1() returns int
external name MyAssembly.StoredProcedures.SqlStoredProcedure1
go

print dbo.SqlStoredProcedure1()

---user defined type---
create type passport
external name MyAssembly.SqlUserDefinedType1
go

-- буквы регионов на русском
declare @p passport
set @p = 'МР232323'
select @p.ToString();
go 