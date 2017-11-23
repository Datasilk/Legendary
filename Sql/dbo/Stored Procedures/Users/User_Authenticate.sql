CREATE PROCEDURE [dbo].[User_Authenticate] 
	@email nvarchar(64) = '',
	@password nvarchar(255) = ''
AS
BEGIN
	SELECT u.*
	FROM Users u
	WHERE email=@email AND [password]=@password
END