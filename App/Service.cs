using Datasilk.Core.Web;

namespace Legendary
{
    public class Service : Request, IService
    {
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

        public override void AddScript(string url, string id = "", string callback = "")
        {
            if (ContainsResource(url)) { return; }
            Scripts.Append("S.util.js.load('" + url + "', '" + id + "', " + (callback != "" ? callback : "null") + ");");
        }

        public override void AddCSS(string url, string id = "")
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