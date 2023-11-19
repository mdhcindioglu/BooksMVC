using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Books.MVC;

public static class HtmlHelpers
{
    public static string ActiveLink(this ViewContext viewContext, string? controller = null, string? action = null, string? area = null, string? page = null)
    {
        var _controller = viewContext.RouteData.Values["Controller"]?.ToString()?.ToLower();
        var _action = viewContext.RouteData.Values["Action"]?.ToString()?.ToLower();
        var _page = viewContext.RouteData.Values["Page"]?.ToString()?.ToLower();
        var _area = viewContext.RouteData.Values["Area"]?.ToString()?.ToLower();

        return
            (!string.IsNullOrEmpty(_controller) && !string.IsNullOrEmpty(_action) && _controller == controller && _action == action) ||
            (!string.IsNullOrEmpty(_area) && !string.IsNullOrEmpty(_page) && _area == area && _page == page)
            ? "nav-link active" : "nav-link";
    }

    public static string RenderRazorViewToString(Controller controller, string viewName, object? model = null)
    {
        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            IViewEngine? viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            ViewEngineResult? viewResult = viewEngine?.FindView(controller.ControllerContext, viewName, false);
            if (viewResult?.View != null)
            {
                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
            }
            return sw.GetStringBuilder().ToString();
        }
    }
}
