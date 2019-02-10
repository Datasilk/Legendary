using System;
using System.Collections.Generic;

namespace Legendary.Common.Platform
{
    public static class Trash
    {
        public static int GetCount(int userId)
        {
            return Query.Trash.GetCount(userId);
        }

        public static Tuple<List<Query.Models.Book>, List<Query.Models.Chapter>, List<Query.Models.Entry>> GetList(int userId)
        {
            return Query.Trash.GetList(userId);
        }

        public static void Empty(int userId)
        {
            Query.Trash.Empty(userId);
        }

        public static void RestoreAll(int userId)
        {
            Query.Trash.RestoreAll(userId);
        }
    }
}
