using System;
using System.Text;

namespace Legendary.Services
{
    public class Books : Service
    {
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
            var view = new View("/Views/Books/list-item.html");
            var books = Query.Books.GetList(User.userId);
            books.ForEach((Query.Models.Book book) =>
            {
                view["id"] = book.bookId.ToString();
                view["title"] = book.title;
                html.Append(view.Render());
            });
            return html.ToString();
        }
    }
}
