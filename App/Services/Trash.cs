using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Legendary.Services
{
    public class Trash : Service
    {
        public Trash(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public string LoadTrash()
        {
            if (!CheckSecurity()) { return AccessDenied(); }

            var scaffold = new Scaffold("/Views/Trash/trash.html");
            var scaffHeader = new Scaffold("/Views/Trash/section-header");
            var scaffBook = new Scaffold("/Views/Trash/item-book.html");
            var scaffChapter = new Scaffold("/Views/Trash/item-chapter.html");
            var scaffEntry = new Scaffold("/Views/Trash/item-entry.html");

            var trash = Common.Platform.Trash.GetList(User.userId);
            var content = new StringBuilder();

            //render list of books
            if(trash.Item1.Count > 0)
            {
                scaffHeader["title"] = "Books";
                content.Append(scaffHeader.Render() + "\n");
                foreach (var book in trash.Item1)
                {
                    var id = book.bookId.ToString();
                    scaffBook["id"] = id;
                    scaffBook["title"] = book.title;
                    scaffBook["checkbox"] = Common.UI.Checkbox.Render("checkbox-" + id, false, "S.trash.select()");
                    content.Append(scaffBook.Render() + "\n");
                }
            }

            //render list of chapters
            if (trash.Item2.Count > 0)
            {
                scaffHeader["title"] = "Chapters";
                content.Append(scaffHeader.Render() + "\n");
                foreach (var chapter in trash.Item2)
                {
                    var id = chapter.bookId + "-" + chapter.chapter;
                    scaffBook["id"] = id;
                    scaffBook["checkbox"] = Common.UI.Checkbox.Render("checkbox-" + id, false, "S.trash.select()");
                    scaffBook["title"] = chapter.title;
                    content.Append(scaffChapter.Render() + "\n");
                }
            }

            //render list of entries
            if (trash.Item3.Count > 0)
            {
                scaffHeader["title"] = "Entries";
                content.Append(scaffHeader.Render() + "\n");
                foreach (var entry in trash.Item3)
                {
                    var id = entry.entryId.ToString();
                    scaffBook["id"] = id;
                    scaffBook["title"] = entry.title;
                    scaffBook["checkbox"] = Common.UI.Checkbox.Render("checkbox-" + id, false, "S.trash.select()");
                    scaffBook["date-created"] = entry.datecreated.ToString("M/dd/yyyy");
                    content.Append(scaffBook.Render() + "\n");
                }
            }

            scaffold["content"] = content.ToString();

            return scaffold.Render();
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
