CREATE TABLE dbo.TUTSupplier
	(
	TSUID int NOT NULL,
	TSUCreditorNumber varchar(50) NOT NULL,
	TSUHomepage nvarchar(300) NOT NULL
	)  ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'Supplier Id'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTSupplier', N'COLUMN', N'TSUID'
GO
DECLARE @v sql_variant 
SET @v = N'Creditor number'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTSupplier', N'COLUMN', N'TSUCreditorNumber'
GO
DECLARE @v sql_variant 
SET @v = N'Homepage Url'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'TUTSupplier', N'COLUMN', N'TSUHomepage'
GO
ALTER TABLE dbo.TUTSupplier ADD CONSTRAINT
	PK_TUTSupplier PRIMARY KEY CLUSTERED 
	(
	TSUID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
