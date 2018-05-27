﻿using System.Linq;
using Microsoft.AspNetCore.Http;
using Datasilk;

public class Routes : Datasilk.Routes
{
    public override Page FromPageRoutes(HttpContext context, string name)
    {
        var isUser = context.Session.Keys.Contains("user");
        switch (name)
        {
            case "": case "home":
                if (isUser == true)
                {
                    return new Legendary.Pages.Dashboard(context);
                }
                else
                {
                    return new Legendary.Pages.Home(context);
                }
                
            case "dashboard": return new Legendary.Pages.Dashboard(context);
            case "login": return new Legendary.Pages.Login(context);
            case "logout": return new Legendary.Pages.Logout(context);
            case "access-denied": return new Legendary.Pages.ErrorCodes.AccessDenied(context);
        }
        return new Page(context);
    }
}