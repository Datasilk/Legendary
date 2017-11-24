using System;
using System.Collections.Generic;

namespace Legendary.Query
{
    class Entries : QuerySql
    {
        public Entries(string connectionString) : base(connectionString)
        {
        }

        public void CreateEntry(int userId, int bookId, DateTime dateCreated, string title, string summary = "", int chapter = 0)
        {
            Sql.ExecuteNonQuery("Entry_Create",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"bookId", bookId },
                    {"chapter", chapter },
                    {"datecreated", dateCreated },
                    {"title", title },
                    {"summary", summary }
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
