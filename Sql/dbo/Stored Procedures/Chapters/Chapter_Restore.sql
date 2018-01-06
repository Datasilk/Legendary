CREATE PROCEDURE [dbo].[Chapter_Restore]
	@bookId int,
	@chapter int
AS
	UPDATE Chapters SET deleted=0 WHERE bookId=@bookId AND chapter=@chapter
