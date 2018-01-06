using System;
using System.Collections.Generic;
using System.Text;

namespace Legendary.Query
{
    class Markdown:QuerySql
    {
        public Markdown(string connectionString) : base(connectionString)
        {
        }

        public string GetDetails(int entryId, int historicalId = 0)
        {
            return Sql.ExecuteScalar<string>(
                "Markdown_GetDetails",
                new Dictionary<string, object>()
                {
                    { "entryId", entryId },
                    { "historicalId", historicalId }
                }
            );
        }

        public List<DateTime> GetHistory(int entryId)
        {
            return Sql.Populate<DateTime>(
                "Markdown_GetHistory",
                new Dictionary<string, object>()
                {
                    {"entryId", entryId }
                }
            );
        }

        public void Update(int userId, int entryId, string markdown, bool historical = false)
        {
            Sql.ExecuteNonQuery("Markdown_Update",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"entryId", entryId },
                    {"markdown", markdown },
                    {"historical", historical }
                }
            );
        }
    }
}
