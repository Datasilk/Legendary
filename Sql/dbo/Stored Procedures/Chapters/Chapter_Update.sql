CREATE PROCEDURE [dbo].[Chapter_Update]
	@bookId int,
	@chapter int,
	@title nvarchar(255),
	@summary nvarchar(255) = ''
AS
	UPDATE Chapters SET title=@title, summary=@summary
	WHERE bookId=@bookId AND chapter=@chapter