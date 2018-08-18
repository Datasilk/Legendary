using System;
using System.Collections.Generic;

namespace Legendary.Query
{
    public class Entries : QuerySql
    {
        public int CreateEntry(int userId, int bookId, DateTime dateCreated, string title, string summary = "", int chapter = 0, int sort = 0)
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

        public void TrashEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Trash",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
        }

        public void RestoreEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Restore",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
        }

        public void DeleteEntry(int userId, int entryId)
        {
            Sql.ExecuteNonQuery("Entry_Delete",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId }
                }
            );
        }

        public Models.Entry GetDetails(int userId, int entryId)
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

        public Models.Entry GetFirst(int userId, int bookId, int sort = 0)
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

        public List<Models.Entry> GetList(int userId, int bookId, int start = 1, int length = 50, int sort = 0)
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

        public void UpdateBook(int userId, int entryId, int bookId)
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

        public void UpdateChapter(int userId, int entryId, int chapter)
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

        public void UpdateSummary(int userId, int entryId, string summary)
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

        public void UpdateTitle(int userId, int entryId, string title)
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
    }
}
