CREATE TABLE [dbo].[Users]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [usertype] TINYINT NULL,
    [name] NVARCHAR(64) NOT NULL, 
    [email] NVARCHAR(64) NOT NULL, 
    [password] NVARCHAR(255) NOT NULL DEFAULT '', 
    [photo] BIT NOT NULL DEFAULT 0, 
    [active] BIT NOT NULL DEFAULT 1, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE()
)
