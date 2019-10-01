namespace Legendary.Common.UI
{
    public static class Checkbox
    {
        public static string Render(string Id, bool isChecked = false, string onClick = "")
        {
            var scaffold = new Scaffold("/Views/Shared/UI/checkbox.html");
            scaffold["id"] = Id;
            scaffold["checked"] = isChecked == true ? "1" : "";
            scaffold["onclick"] = onClick;
            return scaffold.Render();
        }
    }
}
