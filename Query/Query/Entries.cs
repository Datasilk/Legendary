using System;
using System.Collections.Generic;

namespace Query
{
    public static class Entries
    {
        public static int CreateEntry(int userId, int bookId, DateTime dateCreated, string title, string summary = "", int chapter = 0, int sort = 0)
        {
            return Sql.ExecuteScalar<int>("Entry_Create",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"bookId", bookId },
                    {"chapter", chapter },
                    {"sort", sort },
                    {"datecreated", dateCreated },
                    {"title", title },
                    {"summary", summary }
                }
            );
        }

        public static int TrashEntry(int userId, int entryId)
        {
            return Sql.ExecuteScalar<int>("Entry_Trash",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
        }

        public static void RestoreEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Restore",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
        }

        public static void DeleteEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Delete",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
        }

        public static Models.Entry GetDetails(int userId, int entryId)
        {
            var list = Sql.Populate<Models.Entry>(
                "Entry_GetDetails",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }

        public static Models.Entry GetFirst(int userId, int bookId, int sort = 0)
        {
            var list = Sql.Populate<Models.Entry>(
                "Entries_GetFirst",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"bookId", bookId },
                    {"sort", sort }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static List<Models.Entry> GetList(int userId, int bookId, int start = 1, int length = 50, int sort = 0)
        {
            return Sql.Populate<Models.Entry>(
                "Entries_GetList",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"bookId", bookId },
                    {"start", start },
                    {"length", length },
                    {"sort", sort }
                }
            );
        }

        public static void UpdateBook(int userId, int entryId, int bookId)
        {
            Sql.ExecuteNonQuery("Entry_UpdateBook",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId },
                    {"bookId", bookId }
                }
            );
        }

        public static void UpdateChapter(int userId, int entryId, int chapter)
        {
            Sql.ExecuteNonQuery("Entry_UpdateChapter",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId },
                    {"chapter", chapter }
                }
            );
        }

        public static void UpdateSummary(int userId, int entryId, string summary)
        {
            Sql.ExecuteNonQuery("Entry_UpdateSummary",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId },
                    {"summary", summary }
                }
            );
        }

        public static void UpdateTitle(int userId, int entryId, string title)
        {
            Sql.ExecuteNonQuery("Entry_UpdateTitle",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId },
                    {"title", title }
                }
            );
        }

        public static void Update(int entryId, int bookId, DateTime dateCreated, string title, string summary = "", int chapter = 0)
        {
            Sql.ExecuteNonQuery("Entry_Update",
                new Dictionary<string, object>()
                {
                    {"entryId", entryId },
                    {"bookId", bookId },
                    {"chapter", chapter },
                    {"datecreated", dateCreated },
                    {"title", title },
                    {"summary", summary }
                }
            );
        }
    }
}
