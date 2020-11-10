using System.Collections.Generic;
using System.Text;
using Datasilk.Core.Web;
using System.Text.Json;

namespace Legendary
{
    public class Controller : Request, IController
    {
        public StringBuilder Scripts { get; set; } = new StringBuilder();
        public StringBuilder Css { get; set; } = new StringBuilder();
        private List<string> Resources = new List<string>();

        public string title = "Legendary";
        public string description = "";
        public string theme = "default";

        public virtual void Init() { }

        public string AccessDenied<T>() where T : IController
        {
            return IController.AccessDenied<T>(this);
        }

        public string Error<T>() where T : IController
        {
            Context.Response.StatusCode = 500;
            return IController.Error<T>(this);
        }

        public string Error(string message = "Error 500")
        {
            Context.Response.StatusCode = 500;
            return message;
        }

        public string Error404<T>() where T : IController
        {
            Context.Response.StatusCode = 404;
            return IController.Error404<T>(this);
        }

        public string Error404(string message = "Error 404")
        {
            Context.Response.StatusCode = 404;
            return message;
        }

        public string Redirect(string url)
        {
            return "<script language=\"javascript\">window.location.href = '" + url + "';</script>";
        }

        public virtual string Render(string body = "")
        {
            Scripts.Append("<script language=\"javascript\">S.svg.load('/images/icons.svg?v=" + Server.Version + "');</script>");
            var view = new View("/Views/Shared/layout.html");
            view["title"] = title;
            view["description"] = description;
            view["version"] = Server.Version;
            view["language"] = User.language;
            view["theme"] = theme;
            view["head-css"] = Css.ToString();

            //load body
            view["body"] = body;

            //add initialization script
            view["scripts"] = Scripts.ToString();

            return view.Render();
        }

        public override void Dispose()
        {
            if (user != null)
            {
                User.Save();
            }
        }

        public bool CheckSecurity()
        {
            if (User.userId > 0)
            {
                return true;
            }
            return false;
        }

        public string JsonResponse(dynamic obj)
        {
            Context.Response.ContentType = "application/json";
            return JsonSerializer.Serialize(obj);
        }

        public void AddScript(string url, string id = "", string callback = "")
        {
            if (ContainsResource(url)) { return; }
            Scripts.Append("<script language=\"javascript\"" + (id != "" ? " id=\"" + id + "\"" : "") + " src=\"" + url + "\"" +
                (callback != "" ? " onload=\"" + callback + "\"" : "") + "></script>");
        }

        public void AddCSS(string url, string id = "")
        {
            if (ContainsResource(url)) { return; }
            Css.Append("<link rel=\"stylesheet\" type=\"text/css\"" + (id != "" ? " id=\"" + id + "\"" : "") + " href=\"" + url + "\"></link>");
        }

        public bool ContainsResource(string url)
        {
            if (Resources.Contains(url)) { return true; }
            Resources.Add(url);
            return false;
        }
    }
}