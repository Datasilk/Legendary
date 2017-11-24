CREATE PROCEDURE [dbo].[Entry_Delete]
	@userId int,
	@entryId int
AS
	DELETE FROM Entries WHERE entryId=@entryId AND userId=@userId
