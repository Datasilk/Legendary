using Microsoft.AspNetCore.Http;

namespace Legendary
{
    public class Service : Datasilk.Web.Service
    {
        public Service(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }
    }
}