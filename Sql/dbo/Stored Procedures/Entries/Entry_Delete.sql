CREATE PROCEDURE [dbo].[Entry_Delete]
	@userId int,
	@entryId int
AS
	/* permanently deletes an entry & associated markdown records from the trash */
	DELETE FROM Entries WHERE entryId=@entryId AND userId=@userId
