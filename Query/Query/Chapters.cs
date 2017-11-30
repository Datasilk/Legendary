using System;
using System.Collections.Generic;
using System.Text;

namespace Legendary.Query
{
    public class Chapters : QuerySql
    {
        public Chapters(string connectionString) : base(connectionString)
        {
        }

        public void CreateChapter(int bookId, int chapter, string title, string summary)
        {
            Sql.ExecuteNonQuery("Chapter_Create",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId },
                    {"chapter", chapter },
                    {"title", title },
                    {"summary", summary }
                }
            );
        }

        public void DeleteChapter(int bookId, int chapter)
        {
            Sql.ExecuteNonQuery("Chapter_Delete",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId },
                    {"chapter", chapter }
                }
            );
        }

        public void UpdateChapter(int bookId, int chapter, string title, string summary)
        {
            Sql.ExecuteNonQuery("Chapter_Update",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId },
                    {"chapter", chapter },
                    {"title", title },
                    {"summary", summary }
                }
            );
        }

        public int GetMax(int bookId)
        {
            return Sql.ExecuteScalar<int>("Chapter_GetMax", new Dictionary<string, object>() { { "bookId", bookId } });
        }

        public List<Models.Chapter> GetList(int bookId)
        {
            return Sql.Populate<Models.Chapter>(
                "Chapters_GetList",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId }
                }
            );
        }
    }
}
