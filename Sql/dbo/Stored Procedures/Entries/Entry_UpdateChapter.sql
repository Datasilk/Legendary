CREATE PROCEDURE [dbo].[Entry_UpdateChapter]
	@userId int,
	@entryId int,
	@chapter int
AS
	UPDATE Entries SET chapter=@chapter
	WHERE entryId=@entryId AND userId=@userId