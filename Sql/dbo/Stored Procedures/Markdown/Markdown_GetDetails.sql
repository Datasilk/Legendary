CREATE PROCEDURE [dbo].[Markdown_GetDetails]
	@entryId int,
	@historicalId int = 0
AS
	SELECT markdown FROM Markdown WHERE entryId=@entryId AND historicalId=@historicalId
