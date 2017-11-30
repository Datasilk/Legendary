CREATE PROCEDURE [dbo].[Books_GetList]
	@userId int,
	@sort int = 0
AS
	SELECT * FROM Books 
	WHERE userId=@userId 
	ORDER BY 
	CASE WHEN @sort = 0 THEN sort END ASC,
	CASE WHEN @sort = 1 THEN title END ASC