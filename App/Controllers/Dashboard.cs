using Microsoft.AspNetCore.Http;
using System.Text;
using Legendary.Common.Platform;

namespace Legendary.Pages
{
    public class Dashboard : Page
    {
        public Dashboard(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (!CheckSecurity()) { return AccessDenied(true, new Login(context)); }

            //add scripts to page
            AddScript("/js/dashboard.js");
            AddCSS("/css/dashboard.css");

            var dash = new Scaffold("/Views/Dashboard/dashboard.html", Server.Scaffold);

            //get list of books
            var html = new StringBuilder();
            var books = Query.Books.GetList(User.userId);
            if(books.Count > 0)
            {
                //books exist
                var list = new Scaffold("/Views/Books/list-item.html", Server.Scaffold);
                var i = 0;
                books.ForEach((Query.Models.Book book) =>
                {
                    if (i == 0)
                    {
                        list.Data["selected"] = "selected";
                    }
                    else
                    {
                        list.Data["selected"] = "";
                    }
                    list.Data["id"] = book.bookId.ToString();
                    list.Data["title"] = book.title;
                    html.Append(list.Render());
                    i++;
                });
                dash.Data["books"] = html.ToString();

                //get list of entries for top book
                var bookId = 0;
                var entryId = 0;
                if (books.Count > 0)
                {
                    bookId = books[0].bookId;
                    var first = Query.Entries.GetFirst(User.userId, bookId, (int)Entries.SortType.byChapter);
                    entryId = first.entryId;
                    if(first != null)
                    {
                        scripts.Append("<script language=\"javascript\">S.entries.bookId=" + bookId + ";S.editor.entryId=" + entryId.ToString() + ";S.dash.init();</script>");

                        //load content of first entry
                        dash.Data["editor-content"] = Entries.LoadEntry(first.entryId, bookId);
                    }
                    else
                    {
                        dash.Data["no-entries"] = "hide";
                        scripts.Append("<script language=\"javascript\">S.entries.bookId=" + bookId + ";S.entries.noentries();S.dash.init();</script>");
                    }
                }
                dash.Data["entries"] = Entries.GetList(User.userId, bookId, entryId, 1, 50, Entries.SortType.byChapter);
            }
            else
            {
                dash.Data["no-books"] = "hide";
                dash.Data["no-entries"] = "hide";
                dash.Data["no-content"] = Server.LoadFileFromCache("/Views/Dashboard/templates/nobooks.html");
            }

            //get count for tags & trash

            dash.Data["tags-count"] = "0";
            dash.Data["trash-count"] = Trash.GetCount(User.userId).ToString();

            //load script templates (for popups)
            dash.Data["templates"] = 
                Server.LoadFileFromCache("/Views/Dashboard/templates/newbook.html") + 
                Server.LoadFileFromCache("/Views/Dashboard/templates/newentry.html") +
                Server.LoadFileFromCache("/Views/Dashboard/templates/newchapter.html") +
                Server.LoadFileFromCache("/Views/Dashboard/templates/noentries.html");
            
            return base.Render(path, dash.Render(), metadata);
        }
    }
}
