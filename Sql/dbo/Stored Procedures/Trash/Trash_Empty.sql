CREATE PROCEDURE [dbo].[Trash_Empty]
	@userId int
AS
	/* empty a user's trash, deleting all trashed books, chapters, and entries permanently */
	DELETE FROM Entries WHERE userId=@userId AND deleted=1
	DELETE FROM Chapters WHERE bookId IN (SELECT bookId FROM Books WHERE userId=@userId) AND deleted=1
	DELETE FROM Books WHERE userId=@userId AND deleted=1
