select region, cust from ( select offices.region, orders.cust,
rank() over (partition by offices.region order by orders.AMOUNT desc) as sales_rank
from OFFICES inner join salesreps on offices.office = salesreps.REP_OFFICE
inner join orders on salesreps.EMPL_NUM = orders.rep) s
where sales_rank = 3

select region, cust, amount,
amount * 100/max(AMOUNT) over (partition by offices.region order by orders.AMOUNT desc) as sales_amount
from OFFICES inner join salesreps on offices.office = salesreps.REP_OFFICE
inner join orders on salesreps.EMPL_NUM = orders.rep

select region, cust, amount * 100/sales_amount as persents from ( select offices.region, orders.cust, orders.amount,
lag(AMOUNT) over (partition by offices.region order by orders.AMOUNT) as sales_amount
from OFFICES inner join salesreps on offices.office = salesreps.REP_OFFICE
inner join orders on salesreps.EMPL_NUM = orders.rep ) s

WITH sales AS (
SELECT cust_num,
YEAR(ORDER_DATE) AS year,
MONTH(ORDER_DATE) AS month,
SUM(AMOUNT) AS total,
dense_rank() OVER (PARTITION BY cust_num, YEAR(ORDER_DATE) ORDER BY MONTH(ORDER_DATE) DESC) AS rank
FROM CUSTOMERS c JOIN ORDERS o
ON c.CUST_NUM = o.CUST
WHERE (YEAR(ORDER_DATE) = 2007 AND MONTH(ORDER_DATE) >=11)
OR (YEAR(ORDER_DATE) = 2008 AND MONTH(ORDER_DATE) >= 11)
GROUP BY cust_num, YEAR(ORDER_DATE), MONTH(ORDER_DATE)
)
SELECT cust_num,
(MAX(CASE WHEN year = 2008 AND rank = 1 THEN total END)
- MAX(CASE WHEN year = 2007 AND rank = 1 THEN total END)) AS sales_difference
FROM sales GROUP BY cust_num

select * from CUSTOMERS inner join ORDERS on CUSTOMERS.CUST_NUM = ORDERS.CUST
WHERE (YEAR(ORDER_DATE) = 2007 AND MONTH(ORDER_DATE) >=11)
OR (YEAR(ORDER_DATE) = 2008 AND MONTH(ORDER_DATE) >= 11)


