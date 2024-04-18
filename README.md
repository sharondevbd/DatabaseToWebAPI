# DatabaseToWebAPI

#SQL Queryies---------------
use Ap
SELECT * FROM Invoices join Vendors on InvoiceID = Vendors.VendorID WHERE VendorState NOT IN ('CA', 'NV', 'OR') AND InvoiceDate > '2016-01-01'

select * from Invoices where InvoiceDate >= '2015-12-01' AND InvoiceDate <= '2015-12-31'

SELECT *
FROM Vendors
WHERE VendorCity LIKE 'SAN%';

go

SELECT *
FROM vendors
WHERE LOWER (VendorContactFName+VendorContactLName) Like '%[aeiou]%'

SELECT *
FROM Vendors
WHERE VendorState LIKE 'N[A-J]%'
go

SELECT *
FROM Vendors
WHERE VendorState LIKE 'N%[A-J]' OR VendorState LIKE 'N%[Z]'  
go

SELECT *
FROM Vendors
WHERE VendorState LIKE 'N%'
  AND SUBSTRING(VendorState, 2, 1) NOT BETWEEN 'K' AND 'Y';

  go

  Select *
  From Vendors
  order by VendorID
  OFFSET 10 ROWS
  Fetch Next 10 rows only
  go

SELECT i.InvoiceID, i.InvoiceDate, v.VendorName
FROM Invoices AS i
INNER JOIN Vendors AS v ON i.VendorID = v.VendorID
WHERE v.VendorState NOT IN ('CA', 'NV', 'OR')
  AND i.InvoiceDate > '2016-01-01';




--  #### Create A WEBAPI Application To perform following operation
--	1. Write a query to retrieve last 5 those invoices record whose invoice total is equal to the sum of
--payment total &amp; credit total.
--2. Write a query to retrieve those invoices record whose date is later then 01/01/2016 or invoice
--total is more than 500 and invoice total must be greater than sum of payment total and credit
--total.
--3. Write a query to retrieve those invoices whose vendor states are all except ‘CA’, ‘NV’, ‘OR’ and
--invoice dates are later than 01/01/2016. 
--4. Write a query to retrieve invoices from 01/05/2016 to 31/05/2016. 
--5. Write a query to retrieve vendors whose vendor city starts with ‘SAN’.
--6. Write a query to retrieve vendors whose contact name has one of the following characters: a, e,
--i, o, u. 
--7. Write a query to find all vendors whose first letter of state starts with N and the next letter is
--one of A through J. 
--8. Write a query to find all vendors whose first letter of state starts with N and the next letter is
--not in K through Y. 
--9. Write a query to retrieve 11 through 20 records of vendors.
