using System.Text;
using Datasilk.Core.Web;
using Legendary.Common.Platform;

namespace Legendary.Controllers
{
    public class Dashboard : Controller
    {
        public override string Render(string body = "")
        {
            if (!CheckSecurity()) { AccessDenied(IController.LoadController<Login>(this).Render()); }

            //add Scripts to page
            AddScript("/js/dashboard.js?v=" + Server.Version);
            AddCSS("/css/dashboard.css?v=" + Server.Version);

            var dash = new View("/Views/Dashboard/dashboard.html");

            //get list of books
            var html = new StringBuilder();
            var books = Query.Books.GetList(User.userId);
            if(books.Count > 0)
            {
                //books exist
                var list = new View("/Views/Books/list-item.html");
                var i = 0;
                books.ForEach((Query.Models.Book book) =>
                {
                    if (i == 0)
                    {
                        list["selected"] = "selected";
                    }
                    else
                    {
                        list["selected"] = "";
                    }
                    list["id"] = book.bookId.ToString();
                    list["title"] = book.title;
                    html.Append(list.Render());
                    i++;
                });
                dash["books"] = html.ToString();

                //get list of entries for top book
                var bookId = 0;
                var entryId = 0;
                if (books.Count > 0)
                {
                    bookId = books[0].bookId;
                    var first = Query.Entries.GetFirst(User.userId, bookId, (int)Entries.SortType.byChapter);
                    var script = new StringBuilder("<script language=\"javascript\">S.entries.bookId=" + bookId + ";");
                    entryId = first.entryId;
                    
                    if (first != null)
                    {
                        //load content of first entry
                        dash["editor-content"] = Entries.LoadEntry(first.entryId, bookId);
                        script.Append("S.editor.entryId=" + entryId.ToString() + ";$('.editor').removeClass('hide');");
                    }
                    else
                    {
                        dash["no-entries"] = "hide";
                        script.Append("S.entries.noentries();");
                    }
                    Scripts.Append(script.ToString() + "S.dash.init();</script>");
                }
                dash["entries"] = Entries.GetList(User.userId, bookId, entryId, 1, 500, Entries.SortType.byChapter);
            }
            else
            {
                dash["no-books"] = "hide";
                dash["no-entries"] = "hide";
                dash["no-content"] = Server.LoadFileFromCache("/Views/Dashboard/templates/nobooks.html");
            }

            //get count for tags & trash

            dash["tags-count"] = "0";
            dash["trash-count"] = Trash.GetCount(User.userId).ToString();

            //load script templates (for popups)
            dash["templates"] = 
                Server.LoadFileFromCache("/Views/Dashboard/templates/newbook.html") + 
                Server.LoadFileFromCache("/Views/Dashboard/templates/newentry.html") +
                Server.LoadFileFromCache("/Views/Dashboard/templates/newchapter.html") +
                Server.LoadFileFromCache("/Views/Dashboard/templates/noentries.html");
            
            return base.Render(dash.Render());
        }
    }
}
