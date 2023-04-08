using Hadi.Cms.Configuration;
using Hadi.Cms.Repository.UnitOfWork;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Utilities.Authorization
{
    public class CheckIpBannedAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private DataContext _dataContext;

        public CheckIpBannedAttribute()
        {
            _dataContext = new DataContext();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                if (Settings.Application_EnableIpBanned)
                {
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    if (ipAddress != null && IsBannedIp(ipAddress.Trim()))
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    return true;
                }
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

        private bool IsBannedIp(string ipAddress)
        {
            var bannedIps = _dataContext.IpBannedRepository.GetList(b => b.IsActive);
            if (bannedIps.Count > 0)
            {
                foreach (var item in bannedIps)
                {
                    if (item.IpAddress.Trim() == ipAddress)
                        return true;
                }
                return false;
            }
            else
                return false;
        }
    }
}