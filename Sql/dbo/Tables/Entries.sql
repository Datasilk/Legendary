CREATE TABLE [dbo].[Entries]
(
	[entryId] INT NOT NULL PRIMARY KEY, 
    [categoryId] INT NOT NULL DEFAULT 0, 
    [chapter] NCHAR(10) NOT NULL DEFAULT 0, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [datemodified] DATETIME NOT NULL DEFAULT GETDATE(), 
    [title] NVARCHAR(255) NOT NULL, 
    [summary] NVARCHAR(MAX) NOT NULL
)
