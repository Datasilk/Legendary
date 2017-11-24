CREATE PROCEDURE [dbo].[Entry_GetDetails]
	@userId int,
	@entryId int
AS
	SELECT * FROM View_Entries WHERE entryId=@entryId AND userId=@userId
	SELECT * FROM Tags WHERE entryId=@entryId