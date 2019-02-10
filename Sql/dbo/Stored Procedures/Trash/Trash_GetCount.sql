CREATE PROCEDURE [dbo].[Trash_GetCount]
	@userId int
AS
	DECLARE @total int = 0

	SELECT @total += COUNT(*) FROM Books
	WHERE userId=@userId AND deleted=1

	SELECT @total += COUNT(*) FROM Chapters
	WHERE bookId IN (SELECT bookId FROM Books WHERE userId=@userId) 
	AND deleted=1

	SELECT @total += COUNT(*) FROM Entries
	WHERE userId=@userId AND deleted=1

	SELECT @total
