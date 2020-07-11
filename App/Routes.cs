using System.Linq;
using Microsoft.AspNetCore.Http;
using Datasilk.Core.Web;
using Legendary;

public class Routes : Datasilk.Core.Web.Routes
{
    public override IController FromControllerRoutes(HttpContext context, Parameters parameters, string name)
    {
        switch (name)
        {
            case "": case "home": default:
                if (User.Get(context).userId > 0)
                {
                    return new Legendary.Controllers.Dashboard();
                }
                else
                {
                    return new Legendary.Controllers.Login();
                }
                
            case "dashboard": return new Legendary.Controllers.Dashboard();
            case "login": return new Legendary.Controllers.Login();
            case "logout": return new Legendary.Controllers.Logout();
            case "access-denied": return new Legendary.Controllers.ErrorCodes.AccessDenied();
        }
    }
}