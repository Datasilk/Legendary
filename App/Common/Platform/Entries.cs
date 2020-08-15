using System;
using System.IO;
using System.Text;
using Crypto;

namespace Legendary.Common.Platform
{
    

    public static class Entries
    {
        public enum SortType
        {
            byChapter = 0,
            byNewest = 1,
            byOldest = 2,
            byTitle = 3
        }

        public static UInt512 chachaKey
        {
            get {
                return new UInt512(
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
            }
        }

        public static string GetList(int userId, int bookId, int entryId, int start = 1, int length = 500, SortType sort = 0)
        {
            var html = new StringBuilder();
            var entries = new View("/Views/Entries/entries.html");
            var item = new View("/Views/Entries/list-item.html");
            var chapter = new View("/Views/Entries/chapter.html");
            var chapterlist = Query.Chapters.GetList(bookId);
            var list = Query.Entries.GetList(userId, bookId, start, length, (int)sort);
            var chapterInc = -1;
            var entryIndex = 0;
            var book = Query.Books.GetDetails(userId, bookId);
            entries["book-title"] = book.title;

            if (list.Count > 0)
            {
                list.ForEach((Query.Models.Entry entry) =>
                {
                    entryIndex++;
                    if (chapterInc != entry.chapter && sort == 0)
                    {
                        if (entry.chapter > 0)
                        {
                            //display chapter
                            chapter["chapter"] = "Chapter " + entry.chapter.ToString() + ": " +
                                chapterlist.Find((Query.Models.Chapter c) => { return c.chapter == entry.chapter; }).title;
                            html.Append(chapter.Render());
                        }
                        chapterInc = entry.chapter;
                    }
                    item["id"] = entry.entryId.ToString();
                    item["selected"] = entry.entryId == entryId ? "selected" : entryId == 0 && entryIndex == 1 ? "selected" : "";
                    item["title"] = entry.title;
                    item["summary"] = entry.summary;
                    item["date-created"] = entry.datecreated.ToString("M/dd/yyyy");
                    html.Append(item.Render());
                });
                entries["entries"] = html.ToString();
            }
            else
            {
                html.Append(Server.LoadFileFromCache("/Views/Entries/no-entries.html"));
            }

            return entries.Render();
        }

        public static int CreateEntry(int userId, int bookId, string title, string summary, int chapter)
        {
            try
            {
                return Query.Entries.CreateEntry(userId, bookId, DateTime.Now, title, summary, chapter);
            }
            catch (Exception)
            {
                throw new ServiceErrorException("Error creating new entry");
            }
        }

        public static void SaveEntry(int userId, int entryId, string content)
        {
            if(userId == 0) { return; }
            var entry = Query.Entries.GetDetails(userId, entryId);
            var path = "/Content/books/" + entry.bookId + "/";
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }

            // encrypt content using ChaCha20
            var data = GetBytes(content);
            var chacha = new ChaCha20(chachaKey);
            chacha.Transform(data);

            //save encrypted content to file
            File.WriteAllBytes(Server.MapPath(path + entryId + ".dat"), data);
        }

        public static string LoadEntry(int entryId, int bookId)
        {
            var path = "/Content/books/" + bookId + "/";
            var file = Server.MapPath(path + entryId + ".dat");
            if (File.Exists(file))
            {
                try
                {
                    // decrypt content using ChaCha20
                    var data = File.ReadAllBytes(file);
                    var chacha = new ChaCha20(chachaKey);
                    chacha.Transform(data);
                    return Encoding.UTF8.GetString(data).Replace("\0", "");
                }
                catch (IOException)
                {
                    throw new ServiceErrorException("Could not read file");
                }
                catch (Exception)
                {
                    throw new ServiceErrorException("Could not decrypt file");
                }

            }
            return "";
        }

        public static string LoadEntryInfo(int userId, int entryId, int bookId)
        {
            var info = new View("/Views/Dashboard/templates/entryinfo.html");
            var details = Query.Entries.GetDetails(userId, entryId);
            info["title"] = details.title.Replace("\"", "&quot;");
            info["summary"] = details.summary.Replace("\"", "&quot;");
            info["datecreated"] = details.datecreated.ToString("M/dd/yyyy h:mm:ss tt");

            //get list of chapters
            var chapters = new StringBuilder();
            chapters.Append("<option value=\"0\">[No Chapter]</option>");
            Query.Chapters.GetList(bookId).ForEach((Query.Models.Chapter chapter) =>
            {
                chapters.Append("<option value=\"" + chapter.chapter + "\"" + (details.chapter == chapter.chapter ? " selected" : "") + ">" + chapter.chapter + ": " + chapter.title + "</option>");
            });

            //get list of books
            var books = new StringBuilder();
            Query.Books.GetList(bookId).ForEach((Query.Models.Book book) =>
            {
                books.Append("<option value=\"" + book.bookId + "\"" + (book.bookId == bookId ? " selected" : "") + ">" + book.title + "</option>");
            });

            info["chapters"] = chapters.ToString();
            info["books"] = books.ToString();
            return info.Render();
        }

        public static void UpdateEntryInfo(int entryId, int bookId, DateTime datecreated, string title, string summary, int chapter)
        {
            try
            {
                Query.Entries.Update(entryId, bookId, datecreated, title, summary, chapter);
            }
            catch (Exception)
            {
                throw new ServiceErrorException("Error updating existing entry");
            }
        }

        public static int TrashEntry(int userId, int entryId)
        {
            return Query.Entries.TrashEntry(userId, entryId);
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
