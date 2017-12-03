using System;
using System.Collections.Generic;
using System.Text;

namespace Legendary.Pages
{
    public class Dashboard : Page
    {
        public Dashboard(Core LegendaryCore) : base(LegendaryCore)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (!CheckSecurity()) { return AccessDenied(); }

            //add scripts to page
            //AddCSS("/css/utility/font-awesome.css");
            //AddScript("/js/utility/simplemde.min.js");
            //AddCSS("/css/utility/simplemde.min.css");
            //AddScript("/js/utility/highlight.min.js");
            //AddCSS("/css/utility/highlight/atelier-forest-light.css"); // <-- define custom code highlight theme here
            //AddScript("/js/utility/remarkable.min.js");
            //AddScript("/js/pages/dashboard/dashboard.js");
            AddScript("/js/dashboard.js");
            AddCSS("/css/dashboard.css");

            var dash = new Scaffold(S, "/Pages/Dashboard/dashboard.html");

            //get list of books
            var html = new StringBuilder();
            var list = new Scaffold(S, "/Services/Books/list-item.html");
            var query = new Query.Books(S.Server.sqlConnection);
            var books = query.GetList(S.User.userId);
            var i = 0;
            books.ForEach((Query.Models.Book book) =>
            {
                if(i == 0) {
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

            //get count for tags & trash
            var tags = 0;
            var trash = 0;

            dash.Data["tags-count"] = tags.ToString();
            dash.Data["trash-count"] = trash.ToString();

            //load script templates (for popups)
            dash.Data["templates"] = 
                S.Server.LoadFileFromCache("/Pages/Dashboard/templates/newbook.html") + 
                S.Server.LoadFileFromCache("/Pages/Dashboard/templates/newentry.html") +
                S.Server.LoadFileFromCache("/Pages/Dashboard/templates/newchapter.html");

            //get list of entries for top book
            var entries = new Services.Entries(S);
            var bookId = 0;
            if (books.Count > 0)
            {
                bookId = books[0].bookId;
                var first = new Query.Entries(S.Server.sqlConnection).GetFirst(S.User.userId, bookId, (int)Services.Entries.SortType.byChapter);
                scripts += "<script language=\"javascript\">S.entries.bookId=" + bookId + ";S.editor.entryId=" + first.entryId.ToString() + ";</script>";

                //load content of first entry
                dash.Data["editor-content"] = entries.LoadEntry(first.entryId, bookId);
            }
            dash.Data["entries"] = entries.GetList(bookId, 1, 50, Services.Entries.SortType.byChapter);

            return base.Render(path, dash.Render(), metadata);
        }
    }
}
