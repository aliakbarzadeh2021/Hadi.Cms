using Hadi.Cms.Model.Entities;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Utilities.Authorization
{
    public class CheckUserEnableAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                if (!UserHasAccess(SessionData.Current.User))
                    return false;
                return true;
            }
            else
                return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string redirectUrl = "/Error/NotAccess";

                filterContext.HttpContext.Response.Redirect(redirectUrl);
            }

            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool UserHasAccess(User user)
        {
            if (user != null)
            {
                if (user.IsEnable && !user.IsDeleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}