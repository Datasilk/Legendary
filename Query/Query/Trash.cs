using System.Collections.Generic;

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
    }
}
