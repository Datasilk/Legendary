CREATE PROCEDURE [dbo].[Chapter_Delete]
	@bookId int,
	@chapter int
AS
	DELETE FROM Chapters WHERE bookId=@bookId AND chapter=@chapter
	UPDATE Entries SET chapter=0 WHERE bookId=@bookId AND chapter=@chapter
