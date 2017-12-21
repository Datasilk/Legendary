using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Legendary.Services
{
    public class Entries : Service
    {
        public Entries(Core LegendaryCore) : base(LegendaryCore)
        {
        }

        public enum SortType
        {
            byChapter = 0,
            byNewest = 1,
            byOldest = 2,
            byTitle = 3
        }

        public string GetList(int bookId, int start = 1, int length = 50, SortType sort = 0, bool includeCount = false)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var html = new StringBuilder();
            var entries = new Scaffold(S, "/Services/Entries/entries.html");
            var item = new Scaffold(S, "/Services/Entries/list-item.html");
            var chapter = new Scaffold(S, "/Services/Entries/chapter.html");
            var books = new Query.Books(S.Server.sqlConnection);
            var query = new Query.Entries(S.Server.sqlConnection);
            var chapters = new Query.Chapters(S.Server.sqlConnection);
            var chapterlist = chapters.GetList(bookId);
            var list = query.GetList(S.User.userId, bookId, start, length, (int)sort);
            var chapterInc = -1;
            var book = books.GetDetails(S.User.userId, bookId);
            entries.Data["book-title"] = book.title;

            if (list.Count > 0)
            {
                list.ForEach((Query.Models.Entry entry) =>
                {
                    if(chapterInc != entry.chapter && sort == 0)
                    {
                        if(entry.chapter > 0)
                        {
                            //display chapter
                            chapter.Data["chapter"] = "Chapter " + entry.chapter.ToString() + ": " +
                                chapterlist.Find((Query.Models.Chapter c) => { return c.chapter == entry.chapter; }).title;
                            html.Append(chapter.Render());
                        }
                        chapterInc = entry.chapter;
                    }
                    item.Data["id"] = entry.entryId.ToString();
                    item.Data["title"] = entry.title;
                    item.Data["summary"] = entry.summary;
                    item.Data["date-created"] = entry.datecreated.ToString("d/MM/yyyy");
                    html.Append(item.Render());
                });
                entries.Data["entries"] = html.ToString();
            }
            else
            {
                html.Append(S.Server.LoadFileFromCache("/Services/Entries/no-entries.html"));
            }

            return (includeCount == true ? list.Count + "|" : "") + entries.Render();
        }

        public string CreateEntry(int bookId, string title, string summary, int chapter = 0, int sort = 0)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var query = new Query.Entries(S.Server.sqlConnection);
            var entryId = 0;
            try
            {
                entryId = query.CreateEntry(S.User.userId, bookId, DateTime.Now, title, summary, chapter);
            }
            catch (Exception ex)
            {
                return Error();
            }
            return entryId + "|" + GetList(bookId, 1, 50, (SortType)sort);
        }

        public string SaveEntry(int entryId, string content)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var query = new Query.Entries(S.Server.sqlConnection);
            var entry = query.GetDetails(S.User.userId, entryId);
            var path = "/Content/books/" + entry.bookId + "/";
            if (!Directory.Exists(S.Server.MapPath(path)))
            {
                Directory.CreateDirectory(S.Server.MapPath(path));
            }
            File.WriteAllText(S.Server.MapPath(path + entryId + ".md"), content);
            return "success";
        }

        public string LoadEntry(int entryId, int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var path = "/Content/books/" + bookId + "/";
            var file = S.Server.MapPath(path + entryId + ".md");
            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }
            return "";
        }
    }
}
