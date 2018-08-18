using System;
using System.IO;
using System.Text;
using Crypto;
using Utility.Strings;

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

        public static string GetList(Datasilk.Request request, int bookId, int start = 1, int length = 50, SortType sort = 0, bool includeCount = false)
        {
            Server Server = Server.Instance;
            var html = new StringBuilder();
            var entries = new Scaffold("/Views/Entries/entries.html", Server.Scaffold);
            var item = new Scaffold("/Views/Entries/list-item.html", Server.Scaffold);
            var chapter = new Scaffold("/Views/Entries/chapter.html", Server.Scaffold);
            var books = new Query.Books();
            var query = new Query.Entries();
            var chapters = new Query.Chapters();
            var chapterlist = chapters.GetList(bookId);
            var list = query.GetList(request.User.userId, bookId, start, length, (int)sort);
            var chapterInc = -1;
            var book = books.GetDetails(request.User.userId, bookId);
            entries.Data["book-title"] = book.title;

            if (list.Count > 0)
            {
                list.ForEach((Query.Models.Entry entry) =>
                {
                    if (chapterInc != entry.chapter && sort == 0)
                    {
                        if (entry.chapter > 0)
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
                html.Append(Server.LoadFileFromCache("/Views/Entries/no-entries.html"));
            }

            return (includeCount == true ? list.Count + "|" : "") + entries.Render();
        }

        public static int CreateEntry(Datasilk.Request request, int bookId, string title, string summary, int chapter)
        {
            Server Server = Server.Instance;
            var query = new Query.Entries();
            try
            {
                return query.CreateEntry(request.User.userId, bookId, DateTime.Now, title, summary, chapter);
            }
            catch (Exception)
            {
                throw new ServiceErrorException("Error creating new entry");
            }
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

        public static void SaveEntry(Datasilk.Request request, int entryId, string content)
        {
            Server Server = Server.Instance;
            var query = new Query.Entries();
            var entry = query.GetDetails(request.User.userId, entryId);
            var path = "/Content/books/" + entry.bookId + "/";
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }

            // encrypt content using ChaCha20
            var data = content.GetBytes();
            var chacha = new ChaCha20(chachaKey);
            chacha.Transform(data);

            //save encrypted content to file
            File.WriteAllBytes(Server.MapPath(path + entryId + ".dat"), data);
        }
    }
}
