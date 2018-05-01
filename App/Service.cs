using Microsoft.AspNetCore.Http;

namespace Legendary
{
    public class Service : Datasilk.Service
    {
        public Service(HttpContext context) : base(context) { }
    }
}