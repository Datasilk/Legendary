CREATE PROCEDURE [dbo].[Chapter_Create]
	@bookId int,
	@chapter int = 1,
	@title nvarchar(255),
	@summary nvarchar(255) = ''
AS
	INSERT INTO Chapters (bookId, chapter, title, summary)
	VALUES (@bookId, @chapter, @title, @summary)