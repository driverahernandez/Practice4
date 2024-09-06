CREATE DATABASE DB_Practice4_1;

USE DB_Practice4_1;

-- CREATE TABLES

CREATE TABLE Products
(
	ID int identity(1, 1) not null primary key,
	[Name] nvarchar(25) not null,
	Cost decimal(14,2) not null
);

CREATE TABLE Sales
(
	OrderID int identity(1, 1) not null primary key,
	ProductID int not null,
	Total decimal(14,2) not null,
	[Date] datetime not null
);

CREATE TABLE Purchases
(
	PurchaseID int identity(1, 1) not null primary key,
	ProductID int not null,
	Total decimal(14,2) not null,
	[Date] datetime not null
);

-- INSERT VALUES INTO TABLES

INSERT INTO Products 
VALUES 
('Computer', 199.9),
('Phone', 189.9),
('Printer', 159.7),
('Desk', 200.1),
('Chair', 87.4),
('Headphones', 41.1),
('Mouse', 12.5),
('Keyboard', 21.3),
('Table', 30.6),
('Backpack', 11.2);
INSERT INTO Sales
VALUES 
(1, 190.01, '2024-08-05'),
(4, 153.4, '2024-08-04'),
(3, 134.39, '2024-08-23'),
(4, 174.3, '2024-08-12'),
(1, 19074.54, '2024-08-31'),
(6, 345.56, '2024-08-20'),
(1, 45894.93, '2024-08-29'),
(4, 865.65, '2024-08-19'),
(6, 3453.28, '2024-08-16'),
(3, 7621.3, '2024-08-07');
INSERT INTO Purchases
VALUES 
(8, 382.34, '2024-08-14'),
(3, 9832.3, '2024-08-15'),
(6, 478.98, '2024-08-30'),
(4, 4874.7, '2024-08-11'),
(9, 9823.58, '2024-08-15'),
(6, 239.89, '2024-08-04'),
(7, 3298.53, '2024-08-24'),
(8, 32.48, '2024-08-25'),
(4, 52.47, '2024-08-26'),
(7, 454.45, '2024-08-16');
GO

-- CREATE PROCEDURES FOR CRUD OPERATIONS

CREATE PROCEDURE InsertProduct 
(	
	@Name VARCHAR(25),
	@Cost DECIMAL(14,2)
)
AS 
BEGIN
	INSERT INTO Products
	VALUES(@Name, @Cost);
END
GO

CREATE PROCEDURE SelectProduct
(
	@ID INT
)
AS	
BEGIN
	SELECT *
	FROM Products
	WHERE ID = @ID; 
END
GO

CREATE PROCEDURE UpdateProduct
(	
	@ID INT,
	@Name VARCHAR(25),
	@Cost DECIMAL(14,2)
)
AS 
BEGIN
	UPDATE Products
	SET
	[Name] = @Name,
	Cost = @Cost
	WHERE ID = @ID;
END
GO

CREATE PROCEDURE DeleteProduct
(	
	@ID INT
)
AS 
BEGIN
	DELETE FROM Products
	WHERE ID = @ID;
END
GO

-- USE PROCEDURES 

EXEC InsertProduct 'Lollipop', 123.0;
GO
EXEC SelectProduct 11;
GO
EXEC UpdateProduct 11, 'Dress Shoes', 122.0;
GO
EXEC DeleteProduct 11;
GO

-- CREATE VIEW TO SHOW SOLD AND PURCHASED PRODUCTS

CREATE VIEW SoldAndPurchasedProducts AS
	SELECT ID,  COUNT(s.OrderID) AS SoldProducts, COUNT(pu.PurchaseID) AS PurchasedProducts
	FROM Sales s 
		FULL OUTER JOIN 
	Purchases pu 
		ON pu.ProductID = s.ProductID
		INNER JOIN 
	Products p
		ON	p.ID = pu.ProductID OR  p.ID = s.ProductID

	GROUP BY ID


-- CREATE VIEW TO SHOW AVERAGE WEEKLY SALES

CREATE VIEW WeeklySales AS
	WITH base AS 
	(
		SELECT [Date], Total, DATEPART(WEEK, [Date]) AS [Week], Cost
		FROM Products p
			INNER JOIN
		Sales s ON p.ID = s.ProductID
	), 
	base2 AS
	(
		SELECT [Week],
		AVG(Total) OVER (PARTITION BY [Week]) AS AverageSales, 
		MAX(Cost ) OVER (PARTITION BY [Week]) AS MaxCost, 
		MIN(Cost) OVER (PARTITION BY [Week]) AS MinCost
		FROM base

	),
	base3 AS
	(
		SELECT distinct [Week], AverageSales, ID as MostExpensiveProductID, MaxCost, MinCost
		FROM base2
			INNER JOIN 
		Products
			ON Cost = MaxCost
	)
	SELECT DISTINCT [Week], AverageSales, ID AS LeastExpensiveProductID, MostExpensiveProductID
	FROM base3
		INNER JOIN 
	Products
		ON Cost = MinCost;



-- CREATE TEMPORARY TABLE WITH TEXT OUTPUT

CREATE TABLE #SalesInTextFormat
(
	ProductID INT,
	ProductName VARCHAR(25),
	SalesText VARCHAR(25)
)

INSERT INTO #SalesInTextFormat

SELECT ID AS ProductID,  [Name] AS ProductName,
	   SalesText = CASE WHEN count(s.OrderID) > 0 THEN CONCAT('number of sales: ',count(s.OrderID))
						WHEN count(s.OrderID) = 0 THEN 'no products sold'
				   END
FROM Sales s 
	FULL OUTER JOIN 
Products p
	ON	p.ID = s.ProductID
GROUP BY ID, [Name];

go

-- SHOW PURCHASES MADE ON PRODUCTS THAT START WITH THE LETTER L 

EXEC InsertProduct 'Lollipop', 123.0;
GO

SELECT PurchaseID, [Name]
FROM 
Purchases 
	INNER JOIN 
Products 
	ON ProductID = ID
WHERE Products.[Name] LIKE 'L%';
GO
