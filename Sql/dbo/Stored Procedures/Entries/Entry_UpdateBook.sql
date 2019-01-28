CREATE PROCEDURE [dbo].[Entry_UpdateBook]
	@userId int,
	@entryId int,
	@bookId int
AS
	DECLARE @oldbookId int
	SELECT @oldbookId=bookId FROM Entries WHERE entryId=@entryId
	UPDATE Entries SET bookId=@bookId
	WHERE entryId=@entryId AND userId=@userId

	/* fix broken sort order for both the old book and new book */
	EXEC Entries_FixSortOrder @bookId=@oldbookId
	EXEC Entries_FixSortOrder @bookId=@bookId