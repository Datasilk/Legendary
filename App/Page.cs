using Microsoft.AspNetCore.Http;

namespace Legendary
{
    public class Page : Datasilk.Page
    {
        public Page(HttpContext context) : base(context)
        {
            title = "Legendary";
            description = "Open Source Publishing. Collect Research. Write Books. Become A Legend. Do it all with Markdown.";
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            scripts.Append("<script language=\"javascript\">S.svg.load('/themes/default/icons.svg');</script>");
            return base.Render(path, body, metadata);
        }
    }
}