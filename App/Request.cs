namespace Legendary
{
    public class Request : Datasilk.Core.Web.Request
    {
        protected User user;
        public User User
        {
            get
            {
                if (user == null)
                {
                    user = User.Get(Context);
                }
                return user;
            }
            set
            {
                user = value;
            }
        }

        public virtual bool CheckSecurity()
        {
            return true;
        }
    }
}
