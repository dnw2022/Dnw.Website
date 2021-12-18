using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dnw.Website.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static bool IsActiveMenuLink(this IHtmlHelper htmlHelper, params string[] controllerNames)
        {
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var isCurrentController = controllerNames.Any(controllerName => string.Compare(controllerName, currentController, StringComparison.OrdinalIgnoreCase) == 0);

            return isCurrentController;
        }
    }
}