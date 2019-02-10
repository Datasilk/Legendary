namespace Legendary.Common.Platform
{
    public static class Trash
    {
        public static int GetCount(int userId)
        {
            return Query.Trash.GetCount(userId);
        }
    }
}
