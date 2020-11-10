using System.Collections.Generic;
using System.Text;
using Datasilk.Core.Web;

namespace Legendary
{
    public class Service : Request, IService
    {
        protected StringBuilder Scripts = new StringBuilder();
        protected StringBuilder Css = new StringBuilder();
        protected List<string> Resources = new List<string>();
        
        public void Init() { }

        public override void Dispose()
        {
            if(user != null)
            {
                User.Save();
            }
        }

        public string Success()
        {
            return "success";
        }

        public string Empty() { return "{}"; }

        public string AccessDenied(string message = "Error 403")
        {
            Context.Response.StatusCode = 403;
            return message;
        }

        public string Error(string message = "Error 500")
        {
            Context.Response.StatusCode = 500;
            return message;
        }

        public string BadRequest(string message = "Bad Request 400")
        {
            Context.Response.StatusCode = 400;
            return message;
        }

        public void AddScript(string url, string id = "", string callback = "")
        {
            if (ContainsResource(url)) { return; }
            Scripts.Append("S.util.js.load('" + url + "', '" + id + "', " + (callback != "" ? callback : "null") + ");");
        }

        public void AddCSS(string url, string id = "")
        {
            if (ContainsResource(url)) { return; }
            Scripts.Append("S.util.css.load('" + url + "', '" + id + "');");
        }

        public bool ContainsResource(string url)
        {
            if (Resources.Contains(url)) { return true; }
            Resources.Add(url);
            return false;
        }
    }
}