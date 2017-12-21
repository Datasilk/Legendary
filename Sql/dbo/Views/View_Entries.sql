CREATE VIEW [dbo].[View_Entries]
AS
SELECT e.*, 
l.title AS book_title, l.favorite AS book_favorite,
u.[name] AS author_name, u.email AS auther_email, u.photo AS author_photo
FROM Entries e
LEFT JOIN Books l ON l.bookId=e.bookId
LEFT JOIN Users u ON u.userId=e.userId
