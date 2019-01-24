
IF NOT EXISTS (SELECT DB_NAME(database_id) AS [Database] FROM sys.databases WHERE DB_NAME(database_id)='MahalluDatabase')
BEGIN
	CREATE DATABASE MahalluDatabase;
	PRINT 'Database is created successfully.!';
END
ELSE
	PRINT 'Database already exists.!';
GO

USE MahalluDatabase
GO
CREATE TABLE Residence (
    Id int NOT NULL PRIMARY KEY,
	Number nvarchar(10) NOT NULL UNIQUE,
    [Name] nvarchar(124) NOT NULL,
    Guardian int,
    Area nvarchar(124),
);

CREATE TABLE ResidenceMember (
    Id int NOT NULL PRIMARY KEY,
	ResidenceId int FOREIGN KEY REFERENCES Residence(Id),
	[Name] nvarchar(10) NOT NULL,
    DOB nvarchar(124),
    Job nvarchar(124),
    Mobile nvarchar(10),
	Abroad bit,
	Country nvarchar(30)
);

CREATE TABLE Area (
    Id int NOT NULL PRIMARY KEY,
	[Name] nvarchar(max) NULL,
);

delete  Areas


select * From __MigrationHistory
select * from Areas

select * from Residences
select * from ResidenceMembers


-- Father
-- Mother
-- Husband
-- Wife
-- Son
-- Daughter
-- Grandson
-- Granddaughter
-- Brother
-- Sister
-- Brother in law
-- Sister in law
-- Son in law
-- Daughter in law
-- Nephew
-- Niece