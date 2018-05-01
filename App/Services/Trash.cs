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
            var scaffBook = new Scaffold("/Views/Trash/trash-book.html", Server.Scaffold);

            scaffBook.Child("checkbox").Data["label"] = "Book Magic!";

            scaffold.Data["books"] = scaffBook.Render();

            return Inject(new Response()
            {
                selector = ".trash",
                html = scaffold.Render()
            });
        }
    }
}
