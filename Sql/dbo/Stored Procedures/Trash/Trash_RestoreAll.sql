CREATE PROCEDURE [dbo].[Trash_RestoreAll]
	@userId int
AS
	/* restore all trashed books, chapters, and entries from the trash */
	UPDATE Entries SET deleted=0 WHERE userId=@userId AND deleted=1
	UPDATE Chapters SET deleted=0 WHERE bookId IN (SELECT bookId FROM Books WHERE userId=@userId) AND deleted=1
	UPDATE Books SET deleted=0 WHERE userId=@userId AND deleted=1
