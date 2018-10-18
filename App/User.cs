using System;
using Microsoft.AspNetCore.Http;

namespace Datasilk
{
    public partial class User
    {
        public string language = "en";

        public void SetLanguage(string language)
        {
            this.language = language;
            changed = true;
        }

        partial void VendorInit()
        {
            //check for persistant cookie
            if (userId <= 0 && context.Request.Cookies.ContainsKey("authId"))
            {
                var user = Query.Users.AuthenticateUser(context.Request.Cookies["authId"]);
                if (user != null)
                {
                    //persistant cookie was valid, log in
                    LogIn(user.userId, user.email, user.name, user.datecreated, "", 1, user.photo);
                }
            }
        }

        partial void VendorLogIn()
        {
            //create persistant cookie
            var auth = Query.Users.CreateAuthToken(userId);
            var options = new CookieOptions()
            {
                Expires = DateTime.Now.AddMonths(1)
            };

            context.Response.Cookies.Append("authId", auth, options);
        }

        partial void VendorLogOut()
        {
            context.Response.Cookies.Delete("authId");
        }
    }
}
