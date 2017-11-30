CREATE PROCEDURE [dbo].[Entry_UpdateTitle]
	@userId int,
	@entryId int,
	@title nvarchar(255)
AS
	UPDATE Entries SET title=@title, datemodified=GETDATE()
	WHERE entryId=@entryId AND userId=@userId
