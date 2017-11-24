CREATE PROCEDURE [dbo].[Entry_UpdateSummary]
	@userId int,
	@entryId int,
	@summary nvarchar(255) = ''
AS
	UPDATE Entries SET summary=@summary
	WHERE entryId=@entryId AND userId=@userId