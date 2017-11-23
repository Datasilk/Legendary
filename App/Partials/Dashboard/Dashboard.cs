using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Legendary.Partials
{
    public class Dashboard : Page
    {
        private struct menuItem
        {
            public string title;
            public string url;
            public string id;
        }
        private List<menuItem> menuItems = new List<menuItem>();

        #region "Page"
        public Dashboard(Core LegendaryCore, string parentScripts = "", string parentCSS = "") : base(LegendaryCore)
        {
            scripts = parentScripts;
            headCss = parentCSS;
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            //check security first
            if (CheckSecurity() == false) { return AccessDenied(); }

            //setup scaffolding variables
            Scaffold scaffold = new Scaffold(S, "/Partials/Dashboard/dashboard.html");

            //set title
            scaffold.Data["title"] = "Legendary";

            //setup menu
            if (S.Server.config.GetSection("website:dashboard:search").Value.ToLower() == "true") { scaffold.Data["item-1"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:subjects").Value.ToLower() == "true") { scaffold.Data["item-2"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:topics").Value.ToLower() == "true") { scaffold.Data["item-3"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:articles").Value.ToLower() == "true") { scaffold.Data["item-4"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:feeds").Value.ToLower() == "true") { scaffold.Data["item-5"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:downloads").Value.ToLower() == "true") { scaffold.Data["item-6"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:analyzer").Value.ToLower() == "true") { scaffold.Data["item-7"] = "1"; }
            if (S.Server.config.GetSection("website:dashboard:neurons").Value.ToLower() == "true") { scaffold.Data["item-8"] = "1"; }

            //load body
            scaffold.Data["content"] = body;

            //load developer-level menu
            if (S.User.userType <= 1)
            {
                scaffold.Data["dev-menu"] = "1";
            }

            //load admin menu
            if (S.User.userType == 0)
            {
                scaffold.Data["admin-menu"] = "1";
            }

            //load custom menu
            if(menuItems.Count > 0)
            {
                scaffold.Data["menu"] = "<ul class=\"tabs right\">" +
                    string.Join("", 
                        menuItems.Select<menuItem, string>((menuItem item) =>
                        {
                            return "<li class=\"pad-left\"><a href=\"" + (item.url != "" ? item.url : "javascript:") + "\"" +
                                    (item.id != "" ? " id=\"" + item.id + "\"" : "") +
                                    " class=\"button blue\">" + item.title + "</a></li>";
                        }).ToArray()
                    ) + "</ul>";
            }

            //show log in or log out link
            if(S.User.userId > 0)
            {
                scaffold.Data["logout"] = "1";
            }
            else
            {
                scaffold.Data["login"] = "1";
            }

            //include dashboard resources
            AddScript("/js/partials/dashboard/dashboard.js");
            AddCSS("/css/partials/dashboard/dashboard.css");

            //finally, render page
            return base.Render(path, scaffold.Render(), metadata);
        }
        #endregion

        #region "Interface"
        public void AddMenuItem(string id, string title, string url)
        {
            menuItems.Add(new menuItem() { id = id, title = title, url = url });
        }
        #endregion
    }
}
