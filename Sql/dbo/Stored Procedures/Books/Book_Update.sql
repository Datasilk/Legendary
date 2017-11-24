CREATE PROCEDURE [dbo].[Book_Update]
	@userId int,
	@bookId int,
	@title nvarchar(255) = ''
AS
	UPDATE Books SET title=@title WHERE bookId=@bookId AND userId=@userId