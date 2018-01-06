CREATE PROCEDURE [dbo].[Markdown_Update]
	@userId int,
	@entryId int,
	@historical bit = 0,
	@markdown text = ''
AS
	DECLARE @datecreated datetime = GETDATE()

	
	IF (SELECT COUNT(*) FROM Entries WHERE entryId=@entryId AND userId=@userId) > 0 BEGIN
		IF @historical = 0 BEGIN
			IF (SELECT COUNT(*) FROM Markdown WHERE entryId=@entryId AND historicalId=0) = 0 BEGIN
				/* markdown entry does not exist yet */
				INSERT INTO Markdown (entryId, historicalId, datecreated, markdown) 
				VALUES (@entryId, 0, @datecreated, @markdown)
			END ELSE BEGIN
				/* markdown entry already exists */
				UPDATE Markdown SET markdown=@markdown WHERE entryId=@entryId AND historicalId=0
			END
		END ELSE BEGIN
			/* create historical record for entry markdown */
			DECLARE @maxId int = 1
			SELECT @maxId=MAX(historicalId) + 1 FROM Markdown WHERE entryId=@entryId
			INSERT INTO Markdown (entryId, historicalId, datecreated, markdown) 
			VALUES (@entryId, @maxId, @datecreated, @markdown)
		END
	END

