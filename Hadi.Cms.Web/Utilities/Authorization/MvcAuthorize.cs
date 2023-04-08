using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hadi.Cms.Web.Utilities.Authorization
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        private DataContext _dataContext;

        public MvcAuthorizeAttribute()
        {
            _dataContext = new DataContext();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var routeData = httpContext.Request.RequestContext.RouteData;
            string currentAction = routeData.GetRequiredString("action");
            string currentController = routeData.GetRequiredString("controller");
            string currentArea = routeData.DataTokens["area"] as string;

            if (httpContext.Request.IsAuthenticated)
            {
                if (SessionData.Current.User == null)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Session.Abandon();

                    return false;
                }
                else if (!UserIsAuthorized(SessionData.Current.User, currentArea, currentController, currentAction))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string redirectUrl = "/Error/UnauthorizedRequest";

                filterContext.HttpContext.Response.Redirect(redirectUrl);
            }

            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool UserIsAuthorized(User user, string areaName, string controllerName, string actionName)
        {
            if (user.UserRoles.Any(ur => ur.Role.Name.ToLower() == "globaladministrator"))
            {
                return true;
            }
            else
            {
                areaName = areaName ?? "";
                var feature = _dataContext.FeatureRepository.Get(f => f.AreaName == areaName && f.ControllerName == controllerName + "Controller" && f.ActionName == actionName);
                if (feature != null)
                {
                    var featureId = feature.Id;
                    foreach (var item in user.UserRoles)
                    {
                        var roleFeature = _dataContext.RoleFeatureRepository.Get(rf => rf.RoleId == item.RoleId && rf.FeatureId == featureId);
                        if (roleFeature != null)
                            return true;
                    }
                }
                else
                    return false;
            }
            return false;
        }
    }
}