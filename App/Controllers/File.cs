using Datasilk.Core.Web;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Legendary.Controllers
{
    public class File : Controller
    {
        public override string Render(string body = "")
        {
            if (!CheckSecurity()) { return Error404(); }
            var ext = PathParts[3].ToLower();
            var filename = PathParts[2].ToLower() + "." + ext;
            var fullsize = false;
            var img = false;
            var attachment = false;

            //set content type
            switch (ext)
            {
                case "jpg": case "jpeg": case "png": case "gif":
                    Context.Response.ContentType = "image/" + ext.Replace("jpg", "jpeg");
                    img = true;
                    if(PathParts.Length > 4 && PathParts[4].ToLower() == "full")
                    {
                        fullsize = true;
                    }
                    break;
                case "svg":
                    Context.Response.ContentType = "image/svg+xml";
                    attachment = true;
                    break;
                default:
                    Context.Response.ContentType = "application/" + ext;
                    attachment = true;
                    break;
            }

            if (attachment)
            {
                Context.Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            }

            //serve file
            using (FileStream fs = new FileStream(Server.MapPath("/Content/files/" + PathParts[1] + "/" +  (img && !fullsize ? "thumb/" : "") + filename), FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    Context.Response.Body.WriteAsync(ms.ToArray());
                }
            }
            return "";
        }
    }
}
