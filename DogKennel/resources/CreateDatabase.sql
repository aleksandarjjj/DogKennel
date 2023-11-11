	GO
	BEGIN --DROP TABLES CONDITIONALLY
	IF OBJECT_ID(N'dbo.TempTblDogs', N'U')											IS NOT NULL DROP TABLE TempTblDogs;
	IF OBJECT_ID(N'dbo.TempTblDogHealth', N'U')										IS NOT NULL DROP TABLE TempTblDogHealth;
	IF OBJECT_ID(N'dbo.TempTblDogPedigree', N'U')									IS NOT NULL DROP TABLE TempTblDogPedigree;
	IF OBJECT_ID(N'dbo.TblDogHealth', N'U')											IS NOT NULL DROP TABLE TblDogHealth;
	IF OBJECT_ID(N'dbo.TblDogPedigree', N'U')										IS NOT NULL DROP TABLE TblDogPedigree;
	IF OBJECT_ID(N'dbo.TblDogs', N'U')												IS NOT NULL DROP TABLE TblDogs;
	END
	GO

	GO
	BEGIN --CREATE TABLES
	CREATE TABLE TempTblDogs
(	PedigreeID		NVARCHAR(50)	,
	DateOfBirth		NVARCHAR(50)	,
	Alive			NVARCHAR(1)		,
	Sex				NVARCHAR(1)		,
	Colour			NVARCHAR(10)	,
	AK				NVARCHAR(50)	,
	BreedStatus		NVARCHAR(1)		,
	DKTitles		NVARCHAR(200)	,
	Titles			NVARCHAR(200)	,
	[Name]			NVARCHAR(50)	,
	Picture			NVARCHAR(50)	,
	);
	CREATE TABLE TempTblDogHealth
(	PedigreeID		NVARCHAR(50)	,
	HD				NVARCHAR(15)	,
	AD				NVARCHAR(15)	,
	HZ				NVARCHAR(15)	,
	SP				NVARCHAR(15)	,
	);
	CREATE TABLE TempTblDogPedigree
(	PedigreeID		NVARCHAR(50)	,
	Father			NVARCHAR(50)	,
	Mother			NVARCHAR(50)	,
	TattooNo		NVARCHAR(50)	,
	[Owner]			NVARCHAR(50)	,
	);
	CREATE TABLE TblDogs
(	PedigreeID		NVARCHAR(50)	NOT NULL,
	DateOfBirth		NVARCHAR(50)	,
	Alive			NVARCHAR(1)		,
	Sex				NVARCHAR(1)		,
	Colour			NVARCHAR(10)	,
	AK				NVARCHAR(50)	,
	BreedStatus		NVARCHAR(1)		,
	DKTitles		NVARCHAR(200)	,
	Titles			NVARCHAR(200)	,
	[Name]			NVARCHAR(50)	,
	Picture			NVARCHAR(50)	,
	CONSTRAINT PK_TblDogs_PedigreeID PRIMARY KEY (PedigreeID)
	);
	CREATE TABLE TblDogHealth
(	PedigreeID		NVARCHAR(50)	NOT NULL,
	HD				NVARCHAR(15)	,
	AD				NVARCHAR(15)	,
	HZ				NVARCHAR(15)	,
	SP				NVARCHAR(15)	,
	CONSTRAINT PK_TblDogHealth_PedigreeID PRIMARY KEY (PedigreeID),
	CONSTRAINT FK_TblDogHealth_PedigreeID FOREIGN KEY (PedigreeID) REFERENCES TblDogs(PedigreeID)
	);
	CREATE TABLE TblDogPedigree
(	PedigreeID		NVARCHAR(50)	NOT NULL,
	Father			NVARCHAR(50)	,
	Mother			NVARCHAR(50)	,
	TattooNo		NVARCHAR(50)	,
	[Owner]			NVARCHAR(50)	,
	CONSTRAINT PK_TblDogPedigree_PedigreeID PRIMARY KEY (PedigreeID),
	CONSTRAINT FK_TblDogPedigree_PedigreeID FOREIGN KEY (PedigreeID) REFERENCES TblDogs(PedigreeID)
	);
	END
	GO

	GO
	CREATE TRIGGER trBulkInsert_on_TempTblDogs ON TempTblDogs AFTER INSERT
	AS
	DELETE PedigreeID FROM
	(SELECT *, DupRank = ROW_NUMBER() OVER
	(PARTITION BY PedigreeID ORDER BY (SELECT NULL)
	)FROM TempTblDogs) AS PedigreeID WHERE DupRank > 1

	--INSERT INTO TblDogs
	--THE SQL IS FITTED TO ONLY INSERT CONDITIONALLY IF THE ROWS ALREADY EXIST
	INSERT INTO TblDogs
	SELECT * FROM TempTblDogs
	WHERE NOT EXISTS (SELECT PedigreeID FROM TblDogs Where TblDogs.PedigreeID = TempTblDogs.PedigreeID)

	--TRUNCATE TEMPORARY TABLE
	TRUNCATE TABLE TempTblDogs;
	GO

	GO
	CREATE TRIGGER trBulkInsert_on_TempTblDogHealth ON TempTblDogHealth AFTER INSERT
	AS
	--DELETE DUPES
	
	DELETE PedigreeID FROM
	(SELECT *, DupRank = ROW_NUMBER() OVER
	(PARTITION BY PedigreeID ORDER BY (SELECT NULL)
	)FROM TempTblDogHealth) AS PedigreeID WHERE DupRank > 1
	

	--INSERT CONDITIONALLY JOINING TblDogs AND TempTblDogPedigree AND CHECKING FOR ALREADY EXISTING ROWS
	INSERT INTO TblDogHealth
	SELECT TblDogs.PedigreeID, HD, AD, HZ, SP FROM TempTblDogHealth JOIN TblDogs ON TblDogs.PedigreeID = TempTblDogHealth.PedigreeID
	WHERE NOT EXISTS (SELECT PedigreeID FROM TblDogHealth Where TblDogHealth.PedigreeID = TempTblDogHealth.PedigreeID)

	--TRUNCATE TEMPORARY TABLE
	TRUNCATE TABLE TempTblDogHealth;
	GO

	GO
	CREATE TRIGGER trBulkInsert_on_TempTblDogPedigree ON TempTblDogPedigree AFTER INSERT
	AS
	--DELETE DUPES
	
	DELETE PedigreeID FROM
	(SELECT *, DupRank = ROW_NUMBER() OVER
	(PARTITION BY PedigreeID ORDER BY (SELECT NULL)
	)FROM TempTblDogPedigree) AS PedigreeID WHERE DupRank > 1
	
	
	--INSERT CONDITIONALLY JOINING TblDogs AND TempTblDogPedigree AND CHECKING FOR ALREADY EXISTING ROWS
	INSERT INTO TblDogPedigree
	SELECT TblDogs.PedigreeID, Father, Mother, TattooNo, [Owner] FROM TempTblDogPedigree JOIN TblDogs ON TblDogs.PedigreeID = TempTblDogPedigree.PedigreeID
	WHERE NOT EXISTS (SELECT PedigreeID FROM TblDogPedigree Where TblDogPedigree.PedigreeID = TempTblDogPedigree.PedigreeID)

	--TRUNCATE TEMPORARY TABLE
	TRUNCATE TABLE TempTblDogPedigree;
	GO