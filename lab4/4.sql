ALTER TABLE apartments ADD location geography;
ALTER TABLE apartments drop column location;

drop index MySpatialIndex ON apartments;
CREATE SPATIAL INDEX MySpatialIndex ON apartments(location) USING GEOGRAPHY_GRID;
select location from apartments;

DECLARE @searchLocation geography = geography::STPointFromText('POINT(-122.34900 47.65100)', 4326);

SELECT *
FROM apartments WITH(INDEX(MySpatialIndex))
WHERE location.STDistance(@searchLocation) < 1000;

UPDATE apartments SET location = 'POINT(74.3833 30.4061)' where apartment_id = 7
UPDATE apartments SET location = 'POINT(84.3833 20.4061)' where apartment_id = 8
UPDATE apartments SET location = 'POINT(64.3833 40.4061)' where apartment_id = 9
UPDATE apartments SET location = 'POINT(54.3833 60.4061)' where apartment_id = 10
UPDATE apartments SET location = 'POINT(44.3833 10.4061)' where apartment_id = 11
UPDATE apartments SET location = 'POINT(34.3833 20.4061)' where apartment_id = 12
UPDATE apartments SET location = 'POINT(24.3833 30.4061)' where apartment_id = 14

begin
    declare @a geography
    declare @b geography
    set @a=(select location from apartments where apartment_id=7)
    set @b=(select location from apartments where apartment_id=8)
    select @a.STIntersection(@a)    -- высчитывает пересечение двух точек
    select @a.STDistance(@b)        -- высчитывает рассто€ние между двум€ точками
    select @a.STDifference(@b)      -- вычитает одну точку из другой
    select @a.STUnion(@b)           -- объедин€ет две точки в одну
end;

begin
DECLARE @g geometry;  
DECLARE @h geometry;  
SET @g = geometry::STGeomFromText('POLYGON((0 0, 0 2, 2 2, 2 0, 0 0))', 0);  
SET @h = geometry::STGeomFromText('POLYGON((1 1, 3 1, 3 3, 1 3, 1 1))', 0);  
SELECT @g.STIntersection(@h);  
end;

--1 
create database map; --ogr2ogr Цf MSSQLSpatial УMSSQL:server=DESKTOP-6BQAJ6M;database=map;trusted_connection=yesФ УD:\University\бд\labs\lab4\map\map.shpФ Цa_srs УEPSG:4326Ф
use map;
select * from map;

go
declare @g geometry = geometry::STGeomFromText('Point(0 0)', 0);
select @g.STBuffer(5), @g.STBuffer(5).ToString() as WKT from map;


--5 
begin
    declare @b geography=geography::STPointFromText('POINT(-73.9852 40.7575)', 4326);
    declare @a geography=geography::STPointFromText('POINT(-73.9452 60.7235)', 4326);
    declare @refined geography=@a.STUnion(@b);
    select @refined;
end;