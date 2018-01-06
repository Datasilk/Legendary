namespace Legendary.Services
{
    public class Trash : Service
    {
        public Trash(Core DatasilkCore) : base(DatasilkCore)
        {
        }

        public string LoadTrash()
        {
            if (!CheckSecurity()) { return AccessDenied(); }

            var scaffold = new Scaffold(S.Server.MapPath("/Services/Trash/trash.html"), S.Server.Scaffold);
            var scaffBook = new Scaffold(S.Server.MapPath("/Services/Trash/trash-book.html"), S.Server.Scaffold);

            scaffBook.Parent("checkbox").Data["label"] = "Book Magic!";

            scaffold.Data["books"] = scaffBook.Render();

            return Inject(new Response()
            {
                selector = ".trash",
                html = scaffold.Render()
            });
        }
    }
}
