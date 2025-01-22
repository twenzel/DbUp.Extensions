CREATE TABLE dbo.TUTArticle
	(
	TACID int NOT NULL,
	TACSupplier int NOT NULL,
	TACManufacturer int NOT NULL,
	TACModel varchar(255) NOT NULL,
	TACPartNumber varchar(50) NOT NULL,
	TACPurchasePrice decimal(12, 2) NOT NULL,
	TACSalesPrice decimal(12, 2) NOT NULL,
	TACDescription nvarchar(300) NOT NULL,
	TACCreateUser int NOT NULL,
	TACCreateDate datetime NOT NULL,
	TACUpdateUser int NULL,
	TACUpdateDate datetime NULL
	)  ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'Article Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACID'
GO
DECLARE @v sql_variant 
SET @v = N'Supplier ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACSupplier'
GO
DECLARE @v sql_variant 
SET @v = N'Manufacturer'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACManufacturer'
GO
DECLARE @v sql_variant 
SET @v = N'Model name'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACModel'
GO
DECLARE @v sql_variant 
SET @v = N'Part number'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACPartNumber'
GO
DECLARE @v sql_variant 
SET @v = N'Purchase price'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACPurchasePrice'
GO
DECLARE @v sql_variant 
SET @v = N'Sales Price'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACSalesPrice'
GO
DECLARE @v sql_variant 
SET @v = N'Description'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACDescription'
GO
DECLARE @v sql_variant 
SET @v = N'Create UserID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACCreateUser'
GO
DECLARE @v sql_variant 
SET @v = N'Creation date'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACCreateDate'
GO
DECLARE @v sql_variant 
SET @v = N'Update UserID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACUpdateUser'
GO
DECLARE @v sql_variant 
SET @v = N'Update date'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTArticle', N'COLUMN', N'TACUpdateDate'
GO
ALTER TABLE dbo.TUTArticle ADD CONSTRAINT
	PK_TUTArticle PRIMARY KEY CLUSTERED 
	(
	TACID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
