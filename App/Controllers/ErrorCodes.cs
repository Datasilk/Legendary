using Microsoft.AspNetCore.Http;

namespace Legendary.Pages.ErrorCodes
{
    public class AccessDenied : Page
    {
        public AccessDenied(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            return base.Render(path, AccessDenied(), metadata);
        }
    }
}
