CREATE PROCEDURE [dbo].[Trash_GetList]
	@userId int
AS
	SELECT * FROM Books
	WHERE userId=@userId AND deleted=1

	SELECT * FROM Chapters
	WHERE bookId IN (SELECT bookId FROM Books WHERE userId=@userId) 
	AND deleted=1

	SELECT * FROM Entries
	WHERE userId=@userId AND deleted=1
