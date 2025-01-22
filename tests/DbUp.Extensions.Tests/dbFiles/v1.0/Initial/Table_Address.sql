CREATE TABLE [dbo].[TUTAddress](
	[TADID] [int] NOT NULL,
	[TADStreet] [nvarchar](100) NOT NULL,
	[TADZipCode] [varchar](10) NOT NULL,
	[TADCity] [nvarchar](100) NOT NULL,
	[TADCompany] [int] NOT NULL,
	[TADCreateUser] [int] NOT NULL,
	[TADCreateDate] [datetime] NOT NULL,
	[TADUpdateUser] [int] NULL,
	[TADUpdateDate] [datetime] NULL,
 CONSTRAINT [PK_TUTAddress] PRIMARY KEY CLUSTERED 
(
	[TADID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Address ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street value including details like appartment number or similar.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADStreet'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ZipCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADZipCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'City' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADCity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Company number of the address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADCompany'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Id of the user that created the address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADCreateUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the address was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADCreateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User id of the user who last updated the address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADUpdateUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the address was last updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUTAddress', @level2type=N'COLUMN',@level2name=N'TADUpdateDate'
GO


