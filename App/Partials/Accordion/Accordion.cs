
namespace Legendary.Partials
{
    public class Accordion : Page
    {
        public Accordion(Core LegendaryCore) : base(LegendaryCore)
        {
        }

        public string Render(string title, string classNames, string contents, bool expanded = true, bool whiteBg = false)
        {
            var scaffold = new Scaffold(S, "/Partials/Accordion/accordion.html");
            scaffold.Data["title"] = title;
            scaffold.Data["classNames"] = classNames;
            scaffold.Data["contents"] = contents;
            scaffold.Data["expanded"] = expanded == true ? "expanded" : "";
            scaffold.Data["whitebg"] = whiteBg == true ? "white" : "";

            return scaffold.Render();
        }
    }
}
