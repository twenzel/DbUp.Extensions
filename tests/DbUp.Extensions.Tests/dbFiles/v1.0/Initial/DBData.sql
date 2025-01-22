
INSERT INTO [dbo].[TUTAddress]
           ([TADID]
           ,[TADStreet]
           ,[TADZipCode]
           ,[TADCity]
           ,[TADCompany]
           ,[TADCreateUser]
           ,[TADCreateDate]
           ,[TADUpdateUser]
           ,[TADUpdateDate])
     VALUES
           (100001
           , 'Champs-Élysées 1'
           , '75000'
           , 'Paris'
           , 1
           , 9999
           , GETDATE()
           , 9999
           , GETDATE())
	,
		   (100002
           , 'Abbey Road'
           , 'WC2N 5DU'
           , 'London'
           , 1
           , 9999
           , GETDATE()
           , 9999
           , GETDATE())
	,
		   (100003
           , 'Fifth Avenue'
           , '10001'
           , 'New York'
           , 1
           , 9999
           , GETDATE()
           , 9999
           , GETDATE())
		   
GO