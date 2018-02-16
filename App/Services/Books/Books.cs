using System;
using System.Collections.Generic;
using System.Text;

namespace Legendary.Services
{
    public class Books : Service
    {
        public Books(Core LegendaryCore) : base(LegendaryCore)
        {
        }

        public string CreateBook(string title)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var query = new Query.Books(S.Server.sqlConnectionString);
            var bookId = 0;
            try { 
                bookId = query.CreateBook(S.User.userId, title, false);
            }
            catch (Exception)
            {
                return Error();
            }
            return "success|" + bookId + "|" + GetBooksList();
        }

        public string GetBooksList()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var html = new StringBuilder();
            var scaffold = new Scaffold("/Services/Books/list-item.html", S.Server.Scaffold);
            var query = new Query.Books(S.Server.sqlConnectionString);
            var books = query.GetList(S.User.userId);
            books.ForEach((Query.Models.Book book) =>
            {
                scaffold.Data["id"] = book.bookId.ToString();
                scaffold.Data["title"] = book.title;
                html.Append(scaffold.Render());
            });
            return html.ToString();
        }
    }
}
