using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class MenuHelper
    {
        public static string IsActive(this HtmlHelper html, string controllers = "", string actions = "", string areas = "", string[] acceptedFeatures = null, string cssClass = "active")
        {
            string result = String.Empty;
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();
            string currentArea = viewContext.RouteData.DataTokens["area"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            if (String.IsNullOrEmpty(areas))
                areas = currentArea;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();
            string[] acceptedAreas = areas.Trim().Split(',').Distinct().ToArray();

            if (acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) && acceptedAreas.Contains(currentArea))
            {
                result = cssClass;
            }
            if (acceptedFeatures != null && acceptedFeatures[0] != "ALL")
            {
                foreach (var feature in acceptedFeatures)
                {
                    foreach (var area in acceptedAreas)
                    {
                        foreach (var controller in acceptedControllers)
                        {
                            foreach (var action in acceptedActions)
                            {
                                if (feature.Contains(area + "-" + controller + "-" + action))
                                {
                                    return result.Replace(" hidden", "");
                                }
                                else if (!feature.Contains(area + "-" + controller + "-" + action) && !result.Contains("hidden"))
                                {
                                    result = result + " hidden";
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
