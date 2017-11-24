CREATE TABLE [dbo].[Chapters]
(
	[bookId] INT NOT NULL, 
    [chapter] INT NOT NULL, 
    [title] NVARCHAR(255) NOT NULL, 
    [summary] NVARCHAR(255) NOT NULL DEFAULT ''
)
