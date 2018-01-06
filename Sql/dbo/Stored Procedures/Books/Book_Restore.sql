CREATE PROCEDURE [dbo].[Book_Restore]
	@userId int,
	@bookId int
AS
	UPDATE Books SET deleted=0 WHERE bookId=@bookId AND userId=@userId
