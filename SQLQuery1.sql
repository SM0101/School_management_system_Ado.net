USE master

CREATE DATABASE schoolMgtDB
GO

USE schoolMgtDB
GO

CREATE SCHEMA sm
GO


CREATE TABLE sm.Registration
(
	UserName varchar(30) UNIQUE NOT NULL,
	Passcode varchar(30) NOT NULL
);
GO
CREATE TABLE sm.Students
(
	StudentID int NOT NULL  PRIMARY KEY IDENTITY,
	StudentName	varchar(25) NOT NULL,
	BirthDate date NOT NULL,
	Gender varchar(10) NOT NULL,
	Class varchar(10) NOT NULL,
	[Address] nvarchar(40) NOT NULL,
	Photo varchar(100)
);
GO


CREATE PROC sm.sp_StudentInsert
@studentname varchar(25),
@birthdate date,
@gender varchar(10),
@class varchar(10),
@address varchar(40),
@photo varchar(100)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO sm.Students
			VALUES (@studentname,@birthdate,@gender,@class,@address,@photo)
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END;
GO

