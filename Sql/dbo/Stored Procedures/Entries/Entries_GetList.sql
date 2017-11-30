CREATE PROCEDURE [dbo].[Entries_GetList]
	@userId int,
	@bookId int = 0,
	@sort int = 0,
	@start int = 1,
	@length int = 20
AS
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER ( ORDER BY
			book_title ASC,
			CASE WHEN @sort = 0 THEN chapter END ASC,
			CASE WHEN @sort = 0 THEN sort END ASC,
			CASE WHEN @sort = 1 THEN datecreated END DESC,
			CASE WHEN @sort = 2 THEN datecreated END ASC,
			CASE WHEN @sort = 3 THEN title END ASC
		) AS rownum, *
		FROM View_Entries
		WHERE userId=@userId
		AND bookId=CASE WHEN @bookId > 0 THEN @bookId ELSE bookId END
	) AS tbl
	WHERE rownum >= @start AND rownum <= @start + @length
