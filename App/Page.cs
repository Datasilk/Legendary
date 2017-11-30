﻿namespace Legendary
{
    public class Page
    {
        public Core S;
        
        public string title = "Legendary";
        public string description = "";
        public string headCss = "";
        public string colorsCss = "/css/themes/default.css";
        public string favicon = "";
        public string svgIcons = "";
        public string scripts = "";
        public bool useTapestry = true;

        public Page(Core LegendaryCore)
        {
            S = LegendaryCore;
            svgIcons = S.Server.LoadFileFromCache("/content/themes/default/icons.svg");
        }

        public virtual string Render(string[] path, string body = "", object metadata = null)
        {
            //renders HTML layout
            var scaffold = new Scaffold(S, "/layout.html");
            scaffold.Data["title"] = title;
            scaffold.Data["description"] = description;
            scaffold.Data["head-css"] = headCss;
            scaffold.Data["colors-css"] = colorsCss;
            scaffold.Data["favicon"] = favicon;
            scaffold.Data["svg-icons"] = svgIcons;
            scaffold.Data["body"] = body;
            scaffold.Data["use-tapestry"] = useTapestry == true ? "1" : "";
            scaffold.Data["use-tapestry-2"] = useTapestry == true ? "1" : "";

            //add initialization script
            scaffold.Data["scripts"] = scripts;

            return scaffold.Render();
        }

        public bool CheckSecurity()
        {
            if (S.User.userId > 0)
            {
                return true;
            }
                return false;
        }

        public string AccessDenied(bool htmlOutput = true)
        {
            if(htmlOutput == true)
            {
                if (!CheckSecurity())
                {
                    var login = new Pages.Login(S);
                    return login.Render(new string[] { });
                }
                var scaffold = new Scaffold(S, "/Pages/access-denied.html");
                return scaffold.Render();
            }
            return "Access Denied";
        }

        public string Error()
        {
            return "error";
        }

        public string Redirect(string url)
        {
            return "<script language=\"javascript\">window.location.href = '" + url + "';</script>";
        }

        public void AddScript(string url)
        {
            scripts += "<script language=\"javascript\" src=\"" + url + "\"></script>";
        }

        public void AddCSS(string url)
        {
            headCss += "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + url + "\"></link>";
        }
    }
}
