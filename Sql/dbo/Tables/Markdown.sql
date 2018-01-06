CREATE TABLE [dbo].[Markdown]
(
	[entryId] INT NOT NULL PRIMARY KEY, 
    [historicalId] INT NOT NULL DEFAULT 0, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [markdown] TEXT NOT NULL DEFAULT ''
)
