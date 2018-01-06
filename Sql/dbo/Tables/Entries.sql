CREATE TABLE [dbo].[Entries]
(
	[entryId] INT NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [bookId] INT NOT NULL, 
    [chapter] INT NOT NULL DEFAULT 0, 
    [sort] INT NOT NULL DEFAULT 0, 
    [deleted] BIT NOT NULL DEFAULT 0, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [datemodified] DATETIME NOT NULL DEFAULT GETDATE(), 
    [title] NVARCHAR(255) NOT NULL, 
    [summary] NVARCHAR(255) NOT NULL, 
    [location] NVARCHAR(255) NOT NULL DEFAULT '', 
    [author] NVARCHAR(50) NOT NULL DEFAULT ''
)
