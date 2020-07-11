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
                var user = User.Get(context);
                if (user.userId > 0)
                {
                    var dash = new Legendary.Controllers.Dashboard();
                    dash.User = user;
                    return dash;
                }
                else
                {
                    var login = new Legendary.Controllers.Login();
                    login.User = user;
                    return login;
                }
                
            case "dashboard": return new Legendary.Controllers.Dashboard();
            case "login": return new Legendary.Controllers.Login();
            case "logout": return new Legendary.Controllers.Logout();
            case "access-denied": return new Legendary.Controllers.ErrorCodes.AccessDenied();
        }
    }
}