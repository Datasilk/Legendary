CREATE PROCEDURE [dbo].[Entry_Restore]
	@userId int,
	@entryId int
AS
	/* restores an entry from the trash */
	UPDATE Entries SET deleted=0 WHERE entryId=@entryId AND userId=@userId
