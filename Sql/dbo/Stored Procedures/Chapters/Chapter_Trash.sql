CREATE PROCEDURE [dbo].[Chapter_Trash]
	@bookId int,
	@chapter int,
	@entries bit = 0
AS
	/* send chapter to the trash, and optionally, send all associated entries to the trash as well */
	UPDATE Chapters SET deleted=1 WHERE bookId=@bookId AND chapter=@chapter
	IF @entries = 1 BEGIN
		UPDATE Entries SET deleted=1 WHERE bookId=@bookId AND chapter=@chapter
	END