select * from clients;
select * from bill_details;
select * from rents;
select * from apartments;

--1.
create table Report(
  id int generated always as identity,
  xml_ xmltype
);
select * from apartments;
select * from rents;

--2.
create or replace function gen_xml
return xmltype 
as
  xml xmltype;
  a      NVARCHAR2(500);
begin
  a:='select * from  clients
inner join rents on rents.client_passport_number = clients.passport_number
inner join apartments on apartments.apartment_id = rents.apartment_id where 
rownum = 1';        
  select xmlelement("XML",
      xmlelement(evalname('clients'),
      dbms_xmlgen.getxmltype(a)))
  into xml
  from dual;
  return xml;
end gen_xml;

select gen_xml from dual;

--3.	
CREATE OR REPLACE PROCEDURE INSERTXML_PROC(
    id integer,
    x IN XMLTYPE)
IS
BEGIN
  INSERT INTO Report (xml_) values (x);
  COMMIT;
END;

begin 
    INSERTXML_PROC(1, gen_xml);
end;

select * from Report;
select r.xml_.GETSTRINGVAL() from Report r;

--4.
drop index xml_index;
CREATE INDEX xml_index ON Report (EXTRACTVALUE(XML_, '/XML/clients/ROWSET/ROW/PASSPORT_NUMBER[0]'));

drop INDEX report_id_idx;
CREATE INDEX report_id_idx ON Report(id);

EXPLAIN PLAN FOR
SELECT *
FROM Report
WHERE id = 1;

SELECT * FROM TABLE(DBMS_XPLAN.DISPLAY);

--5.	
create or replace procedure FINDXML(aa out VARCHAR2) 
is
begin
      select r.xml_.GETSTRINGVAL() xml
      into aa from Report r
      where r.xml_.EXISTSNODE('/XML/clients/ROWSET/ROW/PASSPORT_NUMBER[0]')=1 and r.id=1;
end FINDXML;

DECLARE 
    word VARCHAR2(4000); 
BEGIN
  FINDXML(:word);
  DBMS_OUTPUT.PUT_LINE(:word);
END;