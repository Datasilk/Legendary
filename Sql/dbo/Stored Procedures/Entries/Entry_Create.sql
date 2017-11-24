CREATE PROCEDURE [dbo].[Entry_Create]
	@userId int,
	@bookId int,
	@chapter int = 0,
	@datecreated datetime = null,
	@title nvarchar(255),
	@summary nvarchar(255) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequenceEntries
	IF @datecreated IS NULL BEGIN SET @datecreated = GETDATE() END
	INSERT INTO Entries (entryId, userId, bookId, chapter, datecreated, datemodified, title, summary)
	VALUES (@id, @userId, @bookId, @chapter, @datecreated, @datecreated, @title, @summary)

	SELECT @id