CREATE PROCEDURE [dbo].[Book_UpdateFavorite]
	@userId int,
	@bookId int,
	@favorite bit = 0
AS
	UPDATE Books SET favorite=@favorite WHERE bookId=@bookId AND userId=@userId
