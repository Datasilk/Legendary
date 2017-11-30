CREATE PROCEDURE [dbo].[Book_Delete]
	@userId int,
	@bookId int
AS
	DELETE FROM Books WHERE bookId=@bookId AND userId=@userId
	DELETE FROM Entries WHERE bookId=@bookId AND userId=@userId
