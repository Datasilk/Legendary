CREATE PROCEDURE [dbo].[Entry_UpdateBook]
	@userId int,
	@entryId int,
	@bookId int
AS
	UPDATE Entries SET bookId=@bookId
	WHERE entryId=@entryId AND userId=@userId