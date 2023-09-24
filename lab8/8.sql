-- 1. drop user lob_user cascade;
alter session set "_ORACLE_SCRIPT"=true;

drop tablespace blob_lab;
-- from sys
CREATE TABLESPACE blob_lab
    DATAFILE 'mytbs_BLOB.dbf'
    SIZE 1000 m
    AUTOEXTEND ON NEXT 500M
    MAXSIZE 2000M
    EXTENT MANAGEMENT LOCAL;
    
select * from v$tablespace;


-- 2.
create directory bfile_dir as '/opt/oracle/blobs';
select * from all_directories;
drop directory bfile_dir;


-- 3.
CREATE USER lob_user IDENTIFIED BY 1111
    DEFAULT TABLESPACE blob_lab QUOTA UNLIMITED ON blob_lab
    ACCOUNT UNLOCK;
    

grant create table to lob_user;
grant create session, CREATE ANY DIRECTORY, DROP ANY DIRECTORY to lob_user;
grant read, write on directory bfile_dir to lob_user;
commit;
-- connect lob_user/1111

-- 5.
drop table blob_t;

create table blob_t (
    id number primary key not null,
    foto blob not null,
    doc bfile
);


-- 6.
declare
fHnd bfile;
b blob;
srcOffset integer := 1;
dstOffset integer := 1;
begin
dbms_lob.CreateTemporary( b, true );
fHnd := BFilename( 'ORACLE_BASE', '1.jpg' );
dbms_lob.FileOpen( fHnd, DBMS_LOB.FILE_READONLY );
dbms_lob.LoadFromFile( b, fHnd, DBMS_LOB.LOBMAXSIZE, dstOffset, srcOffset );

insert into blob_t values(2, b, BFILENAME( 'ORACLE_BASE', '8.docx'));
commit;
dbms_lob.FileClose( fHnd );
end;

select * from blob_t;