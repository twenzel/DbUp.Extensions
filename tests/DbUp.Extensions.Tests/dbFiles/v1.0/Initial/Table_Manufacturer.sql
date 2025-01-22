CREATE TABLE dbo.TUTManufacturer
	(
	TMAID int NOT NULL,
	TMAName nvarchar(100) NOT NULL,
	TMAReferenceKey varchar(100) NOT NULL,
	TMAAddressId int NOT NULL,
	TMACreateUser int NOT NULL,
	TMACreateDate datetime NOT NULL,
	TMAUpdateUser int NULL,
	TMAUpdateDate datetime NULL
	)  ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'Primary Key'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMAID'
GO
DECLARE @v sql_variant 
SET @v = N'Manufacturer name'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMAName'
GO
DECLARE @v sql_variant 
SET @v = N'Internal reference key'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMAReferenceKey'
GO
DECLARE @v sql_variant 
SET @v = N'Address Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMAAddressId'
GO
DECLARE @v sql_variant 
SET @v = N'Create UserID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMACreateUser'
GO
DECLARE @v sql_variant 
SET @v = N'Creation date'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMACreateDate'
GO
DECLARE @v sql_variant 
SET @v = N'Update UserID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMAUpdateUser'
GO
DECLARE @v sql_variant 
SET @v = N'Update date'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTManufacturer', N'COLUMN', N'TMAUpdateDate'
GO
ALTER TABLE dbo.TUTManufacturer ADD CONSTRAINT
	PK_TUTManufacturer PRIMARY KEY CLUSTERED 
	(
	TMAID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

