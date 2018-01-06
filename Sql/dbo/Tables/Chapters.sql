CREATE TABLE [dbo].[Chapters]
(
	[bookId] INT NOT NULL, 
    [chapter] INT NOT NULL, 
    [deleted] BIT NOT NULL DEFAULT 0, 
    [title] NVARCHAR(255) NOT NULL, 
    [summary] NVARCHAR(255) NOT NULL DEFAULT ''
)
