using Microsoft.AspNetCore.Http;

namespace Legendary.Services
{
    public class Entries : Service
    {
        public Entries(HttpContext context) : base(context) {}

        public string GetList(int bookId, int start = 1, int length = 50, int sort = 0, bool includeCount = false)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Common.Platform.Entries.GetList(this, bookId, start, length, (Common.Platform.Entries.SortType)sort, includeCount);
            
        }

        public string CreateEntry(int bookId, string title, string summary, int chapter = 0, int sort = 0)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                return 
                    Common.Platform.Entries.CreateEntry(this, bookId, title, summary, chapter) + "|" + 
                    Common.Platform.Entries.GetList(this, bookId, 1, 50, (Common.Platform.Entries.SortType)sort);
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }

        public string SaveEntry(int entryId, string content)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Common.Platform.Entries.SaveEntry(this, entryId, content);
            }
            catch (ServiceErrorException)
            {
                return Error();
            }
            return "success";
        }

        public string LoadEntry(int entryId, int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                return Common.Platform.Entries.LoadEntry(entryId, bookId);
            }
            catch (ServiceErrorException ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
