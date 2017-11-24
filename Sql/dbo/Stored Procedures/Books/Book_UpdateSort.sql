CREATE PROCEDURE [dbo].[Book_UpdateSort]
	@userId int,
	@bookId int,
	@sort int = 0
AS
	/* first, increment all sorted indexes +1 past @sort */
	UPDATE Books SET sort=sort+1 WHERE userId=@userId AND sort >= @sort

	/* next, move target book sort order */
	UPDATE Books SET sort=@sort WHERE bookId=@bookId AND userId=@userId

	/* finally, update all books sorting indexes based on newly broken sort order */
	DECLARE @id int, @cursor CURSOR, @inc int = 1
	SET @cursor = CURSOR FOR
	SELECT bookId FROM Books WHERE userId=@userId ORDER BY sort ASC
	OPEN @cursor
	FETCH FROM @cursor INTO @id
	WHILE @@FETCH_STATUS = 0 BEGIN
		UPDATE Books SET sort=@inc WHERE bookId=@id
		SET @inc += 1
	END
	CLOSE @cursor
	DEALLOCATE @cursor
	