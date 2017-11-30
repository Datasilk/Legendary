CREATE PROCEDURE [dbo].[Chapters_GetList]
	@bookId int = 0
AS
	SELECT * FROM Chapters WHERE bookId=@bookId