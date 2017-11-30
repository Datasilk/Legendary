CREATE PROCEDURE [dbo].[Book_Create]
	@userId int,
	@title nvarchar(255) = '',
	@favorite bit = 0,
	@sort int = 0
AS
	DECLARE @id int = NEXT VALUE FOR SequenceBooks
	IF @sort = 0 BEGIN
		SELECT @sort = MAX(sort) + 1 FROM Books WHERE userId=@userId
		IF @sort IS NULL BEGIN SET @sort = 1 END
	END
	INSERT INTO Books (bookId, userId, title, favorite, sort)
	VALUES (@id, @userId, @title, @favorite, @sort)

	SELECT @id
