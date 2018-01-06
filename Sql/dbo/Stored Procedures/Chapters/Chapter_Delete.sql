CREATE PROCEDURE [dbo].[Chapter_Delete]
	@bookId int,
	@chapter int
AS
	/* permanently deletes a chapter from the trash and updates associated entries to use no chapter */
	DELETE FROM Chapters WHERE bookId=@bookId AND chapter=@chapter
	UPDATE Entries SET chapter=0 WHERE bookId=@bookId AND chapter=@chapter
