CREATE PROCEDURE [dbo].[Book_Trash]
	@userId int,
	@bookId int
AS
	UPDATE Books SET deleted=1 WHERE bookId=@bookId AND userId=@userId
