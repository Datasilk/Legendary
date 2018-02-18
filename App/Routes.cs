using Datasilk;

class Routes : Datasilk.Routes
{
    public Routes(Core DatasilkCore) : base(DatasilkCore)
    {
    }

    public override Page FromPageRoutes(string name)
    {
        var isUser = false;
        if(S.User.userId > 0) { isUser = true; }
        switch (name)
        {
            case "": case "home":
                if (isUser == true)
                {
                    return new Legendary.Pages.Dashboard(S);
                }
                else
                {
                    return new Legendary.Pages.Home(S);
                }
                
            case "dashboard": return new Legendary.Pages.Dashboard(S);
            case "login": return new Legendary.Pages.Login(S);
            case "logout": return new Legendary.Pages.Logout(S);
            case "access-denied": return new Legendary.Pages.ErrorCodes.AccessDenied(S);
        }
        return new Page(S);
    }
}