using System;
using System.Collections.Generic;
using Dapper;

namespace Query
{
    public static class Trash
    {
        public static int GetCount(int userId)
        {
            return Sql.ExecuteScalar<int>("Trash_GetCount",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }

        public static Tuple<List<Models.Book>, List<Models.Chapter>, List<Models.Entry>> GetList(int userId)
        {
            using (var sql = new Connection("Trash_GetList",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }))
            {
                var reader = sql.PopulateMultiple();
                var books = reader.Read<Models.Book>().AsList();
                var chapters = reader.Read<Models.Chapter>().AsList();
                var entries = reader.Read<Models.Entry>().AsList();
                reader.Dispose();
                return Tuple.Create(books, chapters, entries);
            }
        }
        public static void Empty(int userId)
        {
            Sql.ExecuteNonQuery("Trash_Empty",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }
        public static void RestoreAll(int userId)
        {
            Sql.ExecuteNonQuery("Trash_RestoreAll",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }
    }
}
