using System.Linq;
using Microsoft.AspNetCore.Http;

public class Routes : Datasilk.Web.Routes
{
    public override Datasilk.Mvc.Controller FromControllerRoutes(HttpContext context, Parameters parameters, string name)
    {
        var isUser = context.Session.Keys.Contains("user");
        switch (name)
        {
            case "": case "home": default:
                if (isUser == true)
                {
                    return new Legendary.Controllers.Dashboard(context, parameters);
                }
                else
                {
                    return new Legendary.Controllers.Home(context, parameters);
                }
                
            case "dashboard": return new Legendary.Controllers.Dashboard(context, parameters);
            case "login": return new Legendary.Controllers.Login(context, parameters);
            case "logout": return new Legendary.Controllers.Logout(context, parameters);
            case "access-denied": return new Legendary.Controllers.ErrorCodes.AccessDenied(context, parameters);
        }
    }
}