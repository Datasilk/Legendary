CREATE PROCEDURE [dbo].[Entries_FixSortOrder]
	@bookId int
AS
	DECLARE @id int, @cursor CURSOR, @inc int = 1
		SET @cursor = CURSOR FOR
		SELECT entryId FROM Entries WHERE bookId=@bookId ORDER BY sort ASC, datemodified DESC
		OPEN @cursor
		FETCH FROM @cursor INTO @id
		WHILE @@FETCH_STATUS = 0 BEGIN
			UPDATE Entries SET sort=@inc WHERE entryId=@id
			SET @inc += 1
			FETCH FROM @cursor INTO @id
		END
		CLOSE @cursor
		DEALLOCATE @cursor
