namespace Legendary.Controllers
{
    public class File : Controller
    {
        public override string Render(string body = "")
        {
            if (!CheckSecurity()) { return Error404(); }
            var filename = PathParts[2].ToLower();
            var ext = filename.Split('.')[^1];

            //set content type
            switch (ext)
            {
                case "jpg": case "jpeg": case "png": case "gif":
                    Context.Response.ContentType = "image/" + ext.Replace("jpg", "jpeg");
                    break;
                case "svg":
                    Context.Response.ContentType = "image/svg+xml";
                    break;
                default:
                    Context.Response.ContentType = "application/" + ext;
                    break;
            }

            //serve file
            return Server.LoadFileFromCache("/Content/files/" + PathParts[1] + "/" + filename);
        }
    }
}
