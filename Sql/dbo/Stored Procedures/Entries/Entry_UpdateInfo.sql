CREATE PROCEDURE [dbo].[Entry_UpdateInfo]
	@userId int,
	@entryId int,
	@location nvarchar(255) = '',
	@author nvarchar(255) = ''
AS
	UPDATE Entries SET [location]=@location, author=@author
	WHERE entryId=@entryId AND userId=@userId