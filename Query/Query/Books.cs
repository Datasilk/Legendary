using System.Collections.Generic;

namespace Query
{
    public static class Books
    {
        public static int CreateBook(int userId, string title, bool favorite, int sort = 0)
        {
            return Sql.ExecuteScalar<int>("Book_Create",
                new {userId, title, favorite, sort }
            );
        }

        public static void TrashBook(int userId, int bookId)
        {
            Sql.ExecuteNonQuery("Book_Trash",
                new {userId, bookId }
            );
        }

        public static void DeleteBook(int userId, int bookId)
        {
            Sql.ExecuteNonQuery("Book_Delete",
                new { userId, bookId }
            );
        }

        public static void UpdateBook(int userId, int bookId, string title)
        {
            Sql.ExecuteNonQuery("Book_Update",
                new { userId, bookId, title }
            );
        }

        public static void UpdateBookFavorite(int userId, int bookId, bool favorite)
        {
            Sql.ExecuteNonQuery("Book_UpdateFavorite",
                new { userId, bookId, favorite }
            );
        }

        public static void UpdateBookSort(int userId, int bookId, int sort)
        {
            Sql.ExecuteNonQuery("Book_UpdateSort",
                new { userId, bookId, sort }
            );
        }

        public static Models.Book GetDetails(int userId, int bookId)
        {
            var list = Sql.Populate<Models.Book>(
                "Book_GetDetails",
                new { userId, bookId }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }

        public static List<Models.Book> GetList(int userId, int sort = 0)
        {
            return Sql.Populate<Models.Book>(
                "Books_GetList",
                new { userId, sort }
            );
        }
    }
}
