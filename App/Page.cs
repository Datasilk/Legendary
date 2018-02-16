namespace Legendary
{
    public class Page : Datasilk.Page
    {
        public Page(global::Core DatasilkCore) : base(DatasilkCore) {
            title = "Legendary";
            description = "Open Source Publishing. Collect Research. Write Books. Become A Legend. Do it all with Markdown.";
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (scripts.IndexOf("S.svg.load") < 0)
            {
                scripts += "<script language=\"javascript\">S.svg.load('/themes/default/icons.svg');</script>";
            }
            return base.Render(path, body, metadata);
        }
    }
}