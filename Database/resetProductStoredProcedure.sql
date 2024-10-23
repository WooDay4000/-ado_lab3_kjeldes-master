DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_testingResetProductData`()
BEGIN

-- disable foreign key constraints first
	set foreign_key_checks=0;
    set sql_safe_updates=0;
	delete from Products;
    
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (1, 'A4CS      ', 'Murach''s ASP.NET 4 Web Programming with C# 2010', 56.5000, 4637);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (2, 'A4VB      ', 'Murach''s ASP.NET 4 Web Programming with VB 2010', 56.5000, 3974);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (3, 'ADC4      ', 'Murach''s ADO.NET 4 with C# 2010', 56.5000, 3756);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (4, 'ADV4      ', 'Murach''s ADO.NET 4 with VB 2010', 56.5000, 4538);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (5, 'CRFC      ', 'Murach''s CICS Desk Reference', 50.0000, 1865);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (6, 'CS10      ', 'Murach''s C# 2010', 56.5000, 5136);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (7, 'DB1R      ', 'DB2 for the COBOL Programmer, Part 1 (2nd Edition)', 42.0000, 4825);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (8, 'DB2R      ', 'DB2 for the COBOL Programmer, Part 2 (2nd Edition)', 45.0000, 621);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (9, 'JAVP      ', 'Murach''s Java Programming', 56.5000, 3455);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (10, 'JSP2      ', 'Murach''s JAVA Servlets and JSP (2nd Edition)', 52.5000, 4999);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (11, 'MCBL      ', 'Murach''s Structured COBOL', 62.5000, 2386);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (12, 'MCCP      ', 'Murach''s CICS for the COBOL Programmer', 54.0000, 2368);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (13, 'MDOM      ', 'Murach''s JavaScript and DOM Scripting', 54.5000, 6937);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (14, 'SQ12      ', 'Murach''s SQL Server 2012', 57.5000, 2465);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (15, 'VB10      ', 'Murach''s Visual Basic 2010', 56.5000, 2193);
INSERT Products (ProductID, ProductCode, Description, UnitPrice, OnHandQuantity) VALUES (16, 'ZJLR      ', 'Murach''s OS/390 and z/os JCL', 62.5000, 677);

-- enable foreign key constraints
	set foreign_key_checks=1;
	set sql_safe_updates=1;
END$$
DELIMITER ;