using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Legendary.Services
{
    public class Trash : Service
    {
        public Trash(HttpContext context) : base(context)
        {
        }

        public string LoadTrash()
        {
            if (!CheckSecurity()) { return AccessDenied(); }

            var scaffold = new Scaffold("/Views/Trash/trash.html", Server.Scaffold);
            var scaffHeader = new Scaffold("/Views/Trash/section-header", Server.Scaffold);
            var scaffBook = new Scaffold("/Views/Trash/item-book.html", Server.Scaffold);
            var scaffChapter = new Scaffold("/Views/Trash/item-chapter.html", Server.Scaffold);
            var scaffEntry = new Scaffold("/Views/Trash/item-entry.html", Server.Scaffold);

            var trash = Common.Platform.Trash.GetList(User.userId);
            var content = new StringBuilder();

            //render list of books
            if(trash.Item1.Count > 0)
            {
                scaffHeader.Data["title"] = "Books";
                content.Append(scaffHeader.Render() + "\n");
                foreach (var book in trash.Item1)
                {
                    scaffBook.Data["id"] = book.bookId.ToString();
                    scaffBook.Data["title"] = book.title;
                    content.Append(scaffBook.Render() + "\n");
                }
            }

            //render list of chapters
            if (trash.Item2.Count > 0)
            {
                scaffHeader.Data["title"] = "Chapters";
                content.Append(scaffHeader.Render() + "\n");
                foreach (var chapter in trash.Item2)
                {
                    scaffBook.Data["id"] = chapter.bookId + "-" + chapter.chapter;
                    scaffBook.Data["title"] = chapter.title;
                    content.Append(scaffChapter.Render() + "\n");
                }
            }

            //render list of entries
            if (trash.Item3.Count > 0)
            {
                scaffHeader.Data["title"] = "Entries";
                content.Append(scaffHeader.Render() + "\n");
                foreach (var entry in trash.Item3)
                {
                    scaffBook.Data["id"] = entry.entryId.ToString();
                    scaffBook.Data["title"] = entry.title;
                    scaffBook.Data["date-created"] = entry.datecreated.ToString("M/dd/yyyy");
                    content.Append(scaffBook.Render() + "\n");
                }
            }

            scaffold.Data["content"] = content.ToString();

            return Inject(new Response()
            {
                selector = ".trash",
                html = scaffold.Render()
            });
        }

        public string Empty()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Common.Platform.Trash.Empty(User.userId);
                return Success();
            }
            catch (Exception)
            {
                return Error();
            }
        }

        public string RestoreAll()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Common.Platform.Trash.RestoreAll(User.userId);
                return Success();
            }
            catch (Exception)
            {
                return Error();
            }
        }
    }
}
