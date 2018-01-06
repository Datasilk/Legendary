CREATE TABLE [dbo].[Books]
(
	[bookId] INT NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [sort] INT NOT NULL DEFAULT 0,
    [favorite] BIT NOT NULL DEFAULT 0, 
    [deleted] BIT NOT NULL DEFAULT 0,
    [title] NVARCHAR(255) NOT NULL
)
