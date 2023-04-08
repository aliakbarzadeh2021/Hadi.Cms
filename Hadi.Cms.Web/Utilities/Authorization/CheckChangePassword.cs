using Hadi.Cms.Model.Entities;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Utilities.Authorization
{
    public class CheckChangePasswordAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                if (UserMostChangePassword(SessionData.Current.User) &&
                    !httpContext.Request.Url.AbsoluteUri.Contains("Admin/Users/ChangePassword"))
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
                string redirectUrl = "/Admin/Users/ChangePassword?userId=" + SessionData.Current.User.Id.ToString();

                filterContext.HttpContext.Response.Redirect(redirectUrl);
            }

            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool UserMostChangePassword(User user)
        {
            if (user != null)
            {
                if (user.MostChangePassword)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}