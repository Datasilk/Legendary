CREATE TABLE [dbo].[Books]
(
	[bookId] INT NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [title] NVARCHAR(255) NOT NULL, 
    [favorite] BIT NOT NULL DEFAULT 0, 
    [sort] INT NOT NULL DEFAULT 0
)
