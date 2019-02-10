CREATE PROCEDURE [dbo].[Entry_Trash]
	@userId int,
	@entryId int
AS
	UPDATE Entries SET deleted=1 WHERE entryId=@entryId AND userId=@userId
	DECLARE @count INT
	EXEC @count = Trash_GetCount @userId=@userId
	SELECT @count
