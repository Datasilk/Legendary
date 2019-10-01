using Microsoft.AspNetCore.Http;

namespace Legendary
{
    public class Controller : Datasilk.Mvc.Controller
    {
        public Controller(HttpContext context, Parameters parameters) : base(context, parameters)
        {
            title = "Legendary";
            description = "Open Source Publishing. Collect Research. Write Books. Become A Legend. Do it all with Markdown.";
            favicon = "/images/favicon.ico";
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            scripts.Append("<script language=\"javascript\">S.svg.load('/images/icons.svg');</script>");
            return base.Render(path, body, metadata);
        }
    }
}