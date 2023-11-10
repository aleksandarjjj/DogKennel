	IF OBJECT_ID(N'dbo.DogHealth', N'U')											IS NOT NULL DROP TABLE DogHealth;
	IF OBJECT_ID(N'dbo.DogPedigree', N'U')											IS NOT NULL DROP TABLE DogPedigree;
	IF OBJECT_ID(N'dbo.Dogs', N'U')													IS NOT NULL DROP TABLE Dogs;

	CREATE TABLE Dogs
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
	--CONSTRAINT PK_Pedigree PRIMARY KEY (PedigreeID)
	);

	CREATE TABLE DogHealth
(	PedigreeID		NVARCHAR(50)	NOT NULL,
	HD				NVARCHAR(15)	,
	AZ				NVARCHAR(15)	,
	HZ				NVARCHAR(15)	,
	SP				NVARCHAR(10)	,
	--CONSTRAINT FK_DogHealth_PedigreeID FOREIGN KEY (PedigreeID) REFERENCES Dogs(PedigreeID)
	);

	CREATE TABLE DogPedigree
(	PedigreeID		NVARCHAR(50)	NOT NULL,
	Father			NVARCHAR(50)	NOT NULL,
	Mother			NVARCHAR(50)	NOT NULL,
	TattooNo		NVARCHAR(50)	,
	[Owner]			NVARCHAR(50)	,
	--CONSTRAINT FK_DogPedigree_PedigreeID FOREIGN KEY (PedigreeID) REFERENCES Dogs(PedigreeID)
	);