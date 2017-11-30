CREATE PROCEDURE [dbo].[Entry_Create]
	@userId int,
	@bookId int,
	@chapter int = 0,
	@sort int = 0,
	@datecreated datetime = null,
	@title nvarchar(255),
	@summary nvarchar(255) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequenceEntries
	IF @datecreated IS NULL BEGIN SET @datecreated = GETDATE() END
	IF @sort = 0 BEGIN
		IF @chapter > 0 BEGIN
			/* sort based on last entry in chapter */
			SELECT @sort = MAX(sort) + 1 FROM Entries WHERE bookId=@bookId AND chapter=@chapter
		END ELSE BEGIN
			/* sort based on last entry in book */
			SELECT @sort = MAX(sort) + 1 FROM Entries WHERE bookId=@bookId
		END
	END
	IF @sort IS NULL BEGIN SET @sort = 1 END

	INSERT INTO Entries (entryId, userId, bookId, chapter, sort, datecreated, datemodified, title, summary)
	VALUES (@id, @userId, @bookId, @chapter, @sort, @datecreated, @datecreated, @title, @summary)

	/* fix broken sort order */
	IF @chapter > 0 BEGIN
		DECLARE @i int, @cursor CURSOR, @inc int = 1
		SET @cursor = CURSOR FOR
		SELECT entryId FROM Entries WHERE bookId=@bookId ORDER BY chapter ASC, sort ASC
		OPEN @cursor
		FETCH FROM @cursor INTO @i
		WHILE @@FETCH_STATUS = 0 BEGIN
			UPDATE Entries SET sort=@inc WHERE entryId=@i
			SET @inc += 1
			FETCH FROM @cursor INTO @i
		END
		CLOSE @cursor
		DEALLOCATE @cursor
	END

	SELECT @id