CREATE PROCEDURE [dbo].[Book_GetDetails]
	@userId int,
	@bookId int
AS
	SELECT * FROM Books WHERE bookId=@bookId AND userId=@userId