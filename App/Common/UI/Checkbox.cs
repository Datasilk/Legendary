namespace Legendary.Common.UI
{
    public static class Checkbox
    {
        public static string Render(string Id, bool isChecked = false, string onClick = "")
        {
            Server Server = Server.Instance;
            var scaffold = new Scaffold("/Views/Shared/UI/checkbox.html", Server.Scaffold);
            scaffold.Data["id"] = Id;
            scaffold.Data["checked"] = isChecked == true ? "1" : "";
            scaffold.Data["onclick"] = onClick;
            return scaffold.Render();
        }
    }
}
