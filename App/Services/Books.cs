using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Legendary.Services
{
    public class Books : Service
    {
        public Books(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public string CreateBook(string title)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var bookId = 0;
            try { 
                bookId = Query.Books.CreateBook(User.userId, title, false);
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
            var scaffold = new Scaffold("/Views/Books/list-item.html");
            var books = Query.Books.GetList(User.userId);
            books.ForEach((Query.Models.Book book) =>
            {
                scaffold["id"] = book.bookId.ToString();
                scaffold["title"] = book.title;
                html.Append(scaffold.Render());
            });
            return html.ToString();
        }
    }
}
