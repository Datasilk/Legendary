CREATE PROCEDURE [dbo].[Entry_Update]
	@entryId int,
	@bookId int,
	@chapter int = 0,
	@datecreated datetime,
	@title nvarchar(255),
	@summary nvarchar(255) = ''
AS
	UPDATE Entries 
	SET bookId=@bookId, chapter=@chapter, datecreated=@datecreated, datemodified=GETDATE(), 
		title=@title, summary=@summary
	WHERE entryId=@entryId

	/* fix broken sort order */
	EXEC Entries_FixSortOrder @bookId=@bookId
