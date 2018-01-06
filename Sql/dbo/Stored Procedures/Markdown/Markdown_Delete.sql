CREATE PROCEDURE [dbo].[Markdown_Delete]
	@userId int,
	@entryId int,
	@historicalId int = 1
AS
	/* deletes a historical markdown entry */
	IF @historicalId > 0 BEGIN
		IF (SELECT COUNT(*) FROM Entries WHERE entryId=@entryId AND userId=@userId) > 0 BEGIN
			DELETE FROM Markdown WHERE entryId=@entryId AND historicalId=@historicalId
		END
	END
