select * from clients;
select * from 

alter table clients add client_type varchar(100);
update clients set client_type = '������� ����'; 

--���������� ������ ��������������� ����� ��� ������������� ���� ������ �� ������:
--�	����� �����;
--�	��������� �� � ����� ������� ����� (� %);
--�	��������� � ���������� ������� ����� (� %).

select distinct type, count(type) over (partition by type) as type_amount,
count(type) over (partition by type) * 100.0 / count(type) over () as sum_percent
FROM rents
WHERE date_begin > CONVERT(date, '2023-01-06') AND date_begin < CONVERT(date, '2023-06-16');

WITH max_count AS (SELECT MAX(count) AS max_count FROM (
SELECT type, COUNT(*) AS count FROM rents WHERE date_begin > CONVERT(date, '2023-01-06') AND date_begin < CONVERT(date, '2023-06-16')
GROUP BY type) subquery)
SELECT type, COUNT(*) * 100.0 / (SELECT max_count FROM max_count) AS max_type_amount
FROM rents WHERE date_begin > CONVERT(date, '2023-01-06') AND date_begin < CONVERT(date, '2023-06-16')
GROUP BY type;

--3.	����������������� ���������� ������� ������������ ROW_NUMBER() 
--��� ��������� ����������� ������� �� �������� (�� 20 ����� �� ������ ��������).

SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY passport_number) AS row_num
FROM clients) AS ranked WHERE row_num BETWEEN 1 AND 20;

--4.	����������������� ���������� ������� ������������ ROW_NUMBER() ��� �������� ����������.
insert into clients values('familia', 'imya', '��111', NULL, NULL);
insert into clients values('familia', 'imya', '��222', NULL, NULL);

select * from clients;

WITH ranked AS (
  SELECT *,
    ROW_NUMBER() OVER (PARTITION BY surname ORDER BY passport_number) AS row_num
  FROM clients
)
DELETE FROM ranked
WHERE row_num > 1;

--������� ��� ������� ���� �������� ����� �� ������ ��������� 6 ������� ���������.
SELECT
  client_type,
  YEAR(date_begin) AS year,
  MONTH(date_begin) AS month,
  SUM(day_cost) AS rental_sum
FROM
(
  SELECT
    c.client_type,
    r.date_begin,
    a.day_cost,
    ROW_NUMBER() OVER (PARTITION BY r.client_passport_number ORDER BY r.date_begin DESC) AS rn
  FROM clients c
  INNER JOIN rents r ON c.passport_number = r.client_passport_number
  INNER JOIN apartments a ON r.apartment_id = a.apartment_id
) subquery
WHERE
  rn <= 6 
GROUP BY
  client_type, YEAR(date_begin), MONTH(date_begin)
ORDER BY
  client_type, YEAR(date_begin), MONTH(date_begin);

  --����� ������ ���� ������������� ���������� ����� ��� ��� ������������� ����? ������� ��� ���� ��������.

  WITH ranked AS (
  SELECT c.client_type, r.type, COUNT(*) AS service_count,
    ROW_NUMBER() OVER (PARTITION BY c.client_type ORDER BY COUNT(*) DESC) AS row_num
  FROM clients c
  INNER JOIN rents r ON c.passport_number = r.client_passport_number
  GROUP BY c.passport_number, c.client_type, r.type
)
SELECT *
FROM ranked r
WHERE r.row_num = 1;
