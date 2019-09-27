USE [$(DatabaseName)];
GO
/********* Start CREATE TABLE **************/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MDM_Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MDM_Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Department] [int] NOT NULL,
 CONSTRAINT [PK_MDM_Users] PRIMARY KEY ([ID])) 
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MDM_Department]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MDM_Department](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MDM_Department] PRIMARY KEY (ID))
END
GO
INSERT INTO [dbo].[MDM_Users] ([Name], [Password] ,[Email] ,[Department])
     VALUES ('admin', 'admin', 'admin@email.com', 1)
GO
SET IDENTITY_INSERT [dbo].[MDM_Department] ON 
GO
INSERT [dbo].[MDM_Department] ([ID], [Name]) VALUES (1, N'Admin')
GO
INSERT [dbo].[MDM_Department] ([ID], [Name]) VALUES (2, N'Development')
GO
INSERT [dbo].[MDM_Department] ([ID], [Name]) VALUES (3, N'HR')
GO
INSERT [dbo].[MDM_Department] ([ID], [Name]) VALUES (4, N'Support')
GO
SET IDENTITY_INSERT [dbo].[MDM_Department] OFF
GO
CREATE PROCEDURE [dbo].[sp_alluser]
AS
BEGIN
	SELECT ur.ID, ur.Name, ur.Email, ur.Department, dp.Name as departName FROM dbo.MDM_users ur INNER JOIN dbo.MDM_Department dp on ur.Department = dp.Id
END
GO
CREATE PROCEDURE [dbo].[sp_createuser]
	@Id int,
	@Name nvarchar(50),
	@Password nvarchar(50),
	@Email nvarchar(50),
	@Department nvarchar(50),
	@action nvarchar(15)
AS
BEGIN
	DECLARE @result int 
	SET @result = 0

	IF @action = 'Save'
	BEGIN
		INSERT INTO dbo.MDM_Users(Name, Password, Email, Department) VALUES (@Name, @Password, @Email, @Department)
	END

	IF @action = 'Update'
	BEGIN
		UPDATE dbo.MDM_Users
		SET Name = @Name,
			Email = @Email,
			Department = @Department
		WHERE id = @Id
	END

	SET @result = @@ROWCOUNT
	SELECT @result

END
GO
CREATE PROCEDURE [dbo].[sp_validateuser]
	@UserName nvarchar(50),
	@Password nvarchar(20)
AS
BEGIN
	DECLARE @result int 
	SET @result = 0
	IF EXISTS(SELECT * FROM	[dbo].[MDM_users] WHERE Name = @UserName)
	BEGIN
		SET @result = 1
	END
	SELECT @result
END
GO
