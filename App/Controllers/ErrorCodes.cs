using Microsoft.AspNetCore.Http;

namespace Legendary.Controllers.ErrorCodes
{
    public class AccessDenied : Controller
    {
        public AccessDenied(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            return base.Render(path, AccessDenied(), metadata);
        }
    }
}
