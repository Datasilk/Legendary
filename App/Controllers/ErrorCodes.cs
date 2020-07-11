namespace Legendary.Controllers.ErrorCodes
{
    public class AccessDenied : Controller
    {
        public override string Render(string body = "")
        {
            return base.Render(AccessDenied());
        }
    }
}
