namespace Legendary.Services
{
    public class User : Service
    {
        public User(Core LegendaryCore) : base(LegendaryCore)
        {
        }

        public string Authenticate(string email, string password)
        {
            if (S.User.LogIn(email, password) == true)
            {
                if(S.User.lastSubjectId == 0)
                {
                    return "success";
                }
                return "success" ;
            }
            return "err";
        }

        public string SaveAdminPassword(string password)
        {
            if (S.Server.resetPass == true)
            {
                S.User.UpdateAdminPassword(password);
                return "success";
            }
            S.Response.StatusCode = 500;
            return "";
        }

        public string CreateAdminAccount(string name, string email, string password)
        {
            if (S.Server.hasAdmin == false && S.Server.environment == Server.enumEnvironment.development)
            {
                S.User.CreateAdminAccount(name, email, password);
                return "success";
            }
            S.Response.StatusCode = 500;
            return "";
        }
    }
}