CREATE PROCEDURE [dbo].[Markdown_GetHistory]
	@entryId int
AS
	SELECT datecreated FROM Markdown WHERE entryId=@entryId ORDER BY datecreated DESC
