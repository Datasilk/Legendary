using System.Collections.Generic;

namespace Query
{
    public static class Chapters
    {
        public static void CreateChapter(int bookId, int chapter, string title, string summary)
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

        public static void TrashChapter(int bookId, int chapter, bool entries = false)
        {
            Sql.ExecuteNonQuery("Chapter_Trash",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId },
                    {"chapter", chapter },
                    {"entries", entries }
                }
            );
        }

        public static void RestoreChapter(int bookId, int chapter)
        {
            Sql.ExecuteNonQuery("Chapter_Restore",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId },
                    {"chapter", chapter }
                }
            );
        }

        public static void DeleteChapter(int bookId, int chapter)
        {
            Sql.ExecuteNonQuery("Chapter_Delete",
                new Dictionary<string, object>()
                {
                    {"bookId", bookId },
                    {"chapter", chapter }
                }
            );
        }

        public static void UpdateChapter(int bookId, int chapter, string title, string summary)
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

        public static int GetMax(int bookId)
        {
            return Sql.ExecuteScalar<int>("Chapter_GetMax", new Dictionary<string, object>() { { "bookId", bookId } });
        }

        public static List<Models.Chapter> GetList(int bookId)
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
