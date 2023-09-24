--1.
CREATE TABLE Report (
id INT IDENTITY(1,1) PRIMARY KEY,
xml_ XML
);
SET IDENTITY_INSERT Report ON;

--2.	
CREATE FUNCTION gen_xml ()
RETURNS XML
AS
BEGIN
DECLARE @xml XML;
SELECT @xml = (
    SELECT SYSDATETIME() AS timestamp,
           c.passport_number,
           c.surname,
           c.name,
           a.apartment_id,
           a.city,
		   a.street,
		   a.house_number,
		   a.room_number,
		   a.day_cost,
           r.date_begin,
           r.date_end
    FROM clients c
    INNER JOIN rents r ON r.client_passport_number = c.passport_number
    INNER JOIN apartments a ON a.apartment_id = r.apartment_id
    FOR XML PATH('row'), ROOT('report')
);
RETURN @xml;
END;

SELECT dbo.gen_xml();

--3.	
CREATE PROCEDURE INSERTXML_PROC
@id INT,
@xml XML
AS
BEGIN
INSERT INTO Report (id, xml_)
VALUES (@id, @xml);
END;

DECLARE @xml XML = dbo.gen_xml();
EXEC INSERTXML_PROC 1, @xml;

select * from Report;

--4.	 
create primary xml index XML_Index on Report(xml_)

drop index Second_XML_Index on Report

create xml index Second_XML_Index on Report(xml_)
using xml index XML_Index for path
SELECT *
FROM Report
WHERE xml_.exist('/report/row/apartment_id') = 1
OPTION (QUERYTRACEON 8649);

--5.
drop procedure findXML;
CREATE PROCEDURE findXML
    @value NVARCHAR(50)
AS
BEGIN
    SELECT
		xml_.value('(/report/row/timestamp)[1]', 'DATE') AS TimeStamp,
    xml_.value('(/report/row/passport_number)[1]', 'NVARCHAR(50)') AS PassportNumber,
    xml_.value('(/report/row/surname)[1]', 'NVARCHAR(50)') AS Surname,
    xml_.value('(/report/row/name)[1]', 'NVARCHAR(50)') AS Name,
    xml_.value('(/report/row/apartment_id)[1]', 'INT') AS ApartmentId,
    xml_.value('(/report/row/city)[1]', 'NVARCHAR(50)') AS City,
    xml_.value('(/report/row/street)[1]', 'NVARCHAR(50)') AS Street,
    xml_.value('(/report/row/house_number)[1]', 'INT') AS HouseNumber,
    xml_.value('(/report/row/room_number)[1]', 'INT') AS RoomNumber,
    xml_.value('(/report/row/day_cost)[1]', 'MONEY') AS DayCost,
    xml_.value('(/report/row/date_begin)[1]', 'DATE') AS DateBegin,
    xml_.value('(/report/row/date_end)[1]', 'DATE') AS DateEnd
    FROM Report
    WHERE xml_.exist('/report/row/*[.=sql:variable("@value")]') = 1
END

EXECUTE findXML 'MP000001';
