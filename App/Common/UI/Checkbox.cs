namespace Legendary.Common.UI
{
    public static class Checkbox
    {
        public static string Render(string Id, bool isChecked = false, string onClick = "")
        {
            var view = new View("/Views/Shared/UI/checkbox.html");
            view["id"] = Id;
            view["checked"] = isChecked == true ? "1" : "";
            view["onclick"] = onClick;
            return view.Render();
        }
    }
}
