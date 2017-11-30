CREATE PROCEDURE [dbo].[Entry_UpdateSort]
	@userId int,
	@entryId int,
	@sort int = 0
AS
	DECLARE @bookId int = 0;
	SELECT @bookId=bookId FROM Entries WHERE entryId=@entryId AND userId=@userId
	IF @bookId > 0 BEGIN
		/* first, increment all sorted indexes +1 past @sort */
		UPDATE Entries SET sort=sort+1 WHERE bookId=@bookId AND sort >= @sort

		/* next, move target entry sort order */
		UPDATE Entries SET sort=@sort WHERE entryId=@entryId AND userId=@userId

		/* finally, update all entries sorting indexes based on newly broken sort order */
		DECLARE @id int, @cursor CURSOR, @inc int = 1
		SET @cursor = CURSOR FOR
		SELECT entryId FROM Entries WHERE bookId=@bookId ORDER BY sort ASC
		OPEN @cursor
		FETCH FROM @cursor INTO @id
		WHILE @@FETCH_STATUS = 0 BEGIN
			UPDATE Entries SET sort=@inc WHERE entryId=@id
			SET @inc += 1
			FETCH FROM @cursor INTO @id
		END
		CLOSE @cursor
		DEALLOCATE @cursor
	END
	