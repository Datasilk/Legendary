using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Crypto;

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

        private UInt512 chachaKey = new UInt512(
            0X03020100U,
            0X07060504U,
            0X0B0A0908U,
            0X0F0E0D0CU,
            0X13121110U,
            0X17161514U,
            0X1B1A1918U,
            0X1F1E1D1CU,
            0X00000000U,
            0X00000000U,
            0X00000000U,
            0X00000000U,
            0X00000000U,
            0X00000000U,
            0X00000000U,
            0X00000000U
        );

        public string GetList(int bookId, int start = 1, int length = 50, SortType sort = 0, bool includeCount = false)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var html = new StringBuilder();
            var entries = new Scaffold("/Services/Entries/entries.html", S.Server.Scaffold);
            var item = new Scaffold("/Services/Entries/list-item.html", S.Server.Scaffold);
            var chapter = new Scaffold("/Services/Entries/chapter.html", S.Server.Scaffold);
            var books = new Query.Books(S.Server.sqlConnectionString);
            var query = new Query.Entries(S.Server.sqlConnectionString);
            var chapters = new Query.Chapters(S.Server.sqlConnectionString);
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
            var query = new Query.Entries(S.Server.sqlConnectionString);
            var entryId = 0;
            try
            {
                entryId = query.CreateEntry(S.User.userId, bookId, DateTime.Now, title, summary, chapter);
            }
            catch (Exception)
            {
                return Error();
            }
            return entryId + "|" + GetList(bookId, 1, 50, (SortType)sort);
        }

        public string SaveEntry(int entryId, string content)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var query = new Query.Entries(S.Server.sqlConnectionString);
            var entry = query.GetDetails(S.User.userId, entryId);
            var path = "/Content/books/" + entry.bookId + "/";
            if (!Directory.Exists(S.Server.MapPath(path)))
            {
                Directory.CreateDirectory(S.Server.MapPath(path));
            }

            // encrypt content using ChaCha20
            var data = S.Util.Str.GetBytes(content);
            var chacha = new ChaCha20(chachaKey);
            chacha.Transform(data);

            //save encrypted content to file
            File.WriteAllBytes(S.Server.MapPath(path + entryId + ".dat"), data);
            return "success";
        }

        public string LoadEntry(int entryId, int bookId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var path = "/Content/books/" + bookId + "/";
            var file = S.Server.MapPath(path + entryId + ".dat");
            if (File.Exists(file))
            {
                // decrypt content using ChaCha20
                var data = File.ReadAllBytes(file);
                var chacha = new ChaCha20(chachaKey);
                chacha.Transform(data);
                return Encoding.UTF8.GetString(data).Replace("\0","");
            }
            return "";
        }
    }
}
