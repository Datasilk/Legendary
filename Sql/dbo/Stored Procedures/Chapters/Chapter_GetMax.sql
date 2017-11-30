CREATE PROCEDURE [dbo].[Chapter_GetMax]
	@bookId int
AS
	DECLARE @max int = 0
	SELECT @max = MAX(chapter) FROM Chapters WHERE bookId=@bookId
	IF @max IS NULL BEGIN SET @max = 0 END
	SELECT @max
