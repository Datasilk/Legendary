using System;
using System.Collections.Generic;

namespace Query
{
    public static class Entries
    {
        public static int CreateEntry(int userId, int bookId, DateTime dateCreated, string title, string summary = "", int chapter = 0, int sort = 0)
        {
            return Sql.ExecuteScalar<int>("Entry_Create",
                new { userId, bookId, chapter, sort, dateCreated, title, summary }
            );
        }

        public static int TrashEntry(int userId, int entryId)
        {
            return Sql.ExecuteScalar<int>("Entry_Trash",
                new { userId, entryId }
            );
        }

        public static void RestoreEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Restore",
                new { userId, entryId }
            );
        }

        public static void DeleteEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Delete",
                new { userId, entryId }
            );
        }

        public static Models.Entry GetDetails(int userId, int entryId)
        {
            var list = Sql.Populate<Models.Entry>(
                "Entry_GetDetails",
                new { userId, entryId }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }

        public static Models.Entry GetFirst(int userId, int bookId, int sort = 0)
        {
            var list = Sql.Populate<Models.Entry>(
                "Entries_GetFirst",
                new { userId, bookId, sort }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static List<Models.Entry> GetList(int userId, int bookId, int start = 1, int length = 50, int sort = 0)
        {
            return Sql.Populate<Models.Entry>(
                "Entries_GetList",
                new { userId, bookId, start, length, sort }
            );
        }

        public static void UpdateBook(int userId, int entryId, int bookId)
        {
            Sql.ExecuteNonQuery("Entry_UpdateBook",
                new { userId, entryId, bookId }
            );
        }

        public static void UpdateChapter(int userId, int entryId, int chapter)
        {
            Sql.ExecuteNonQuery("Entry_UpdateChapter",
                new { userId, entryId, chapter }
            );
        }

        public static void UpdateSummary(int userId, int entryId, string summary)
        {
            Sql.ExecuteNonQuery("Entry_UpdateSummary",
                new { userId, entryId, summary }
            );
        }

        public static void UpdateTitle(int userId, int entryId, string title)
        {
            Sql.ExecuteNonQuery("Entry_UpdateTitle",
                new { userId, entryId, title }
            );
        }

        public static void Update(int entryId, int bookId, DateTime dateCreated, string title, string summary = "", int chapter = 0)
        {
            Sql.ExecuteNonQuery("Entry_Update",
                new { entryId, bookId, chapter, dateCreated, title, summary }
            );
        }
    }
}
