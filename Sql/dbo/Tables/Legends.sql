CREATE TABLE [dbo].[Legends]
(
	[legendId] INT NOT NULL PRIMARY KEY, 
    [title] NVARCHAR(64) NOT NULL, 
    [favorite] BIT NOT NULL DEFAULT 0, 
    [sort] INT NOT NULL DEFAULT 0
)
