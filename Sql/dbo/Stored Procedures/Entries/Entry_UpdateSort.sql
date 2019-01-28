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
		UPDATE Entries SET sort=@sort, datemodified=GETDATE() WHERE entryId=@entryId AND userId=@userId

		/* finally, update all entries sorting indexes based on newly broken sort order */
		EXEC Entries_FixSortOrder @bookId=@bookId
	END
	