using System;
using System.Collections.Generic;
using System.Text;

namespace Legendary.Pages
{
    public class Dashboard : Page
    {
        public Dashboard(Core LegendaryCore) : base(LegendaryCore)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            var scaffold = new Scaffold(S, "/Pages/Dashboard/dashboard.html");

            //get list of books
            var books = new StringBuilder();
            scaffold.Data["books"] = books.ToString();

            //get count for tags & trash
            var tags = 0;
            var trash = 0;

            scaffold.Data["tags-count"] = tags.ToString();
            scaffold.Data["trash-count"] = trash.ToString();

            AddScript("/js/pages/dashboard/dashboard.js");
            AddCSS("/css/pages/dashboard/dashboard.css");
            return base.Render(path, scaffold.Render(), metadata);
        }
    }
}
