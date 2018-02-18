namespace Legendary.Pages.ErrorCodes
{
    public class AccessDenied : Page
    {
        public AccessDenied(Core DatasilkCore) : base(DatasilkCore)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            return base.Render(path, AccessDenied(), metadata);
        }
    }
}
