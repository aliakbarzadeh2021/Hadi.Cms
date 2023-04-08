using System;
using Hadi.Cms.Configuration;
using Hadi.Cms.Repository.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Utilities.Authorization
{
    public class CheckIpRangeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private DataContext _dataContext;

        public CheckIpRangeAttribute()
        {
            _dataContext = new DataContext();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsAuthenticated)
            {
                if (Settings.Application_EnableIpRange)
                {
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    if (ipAddress != null && !IpInRange(ipAddress.Trim()))
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

        private bool IpInRange(string ipAddress)
        {
            var rangeList = _dataContext.IpRangeRepository.GetList(r => r.IsActive);
            if (rangeList.Any())
            {
                /*foreach (var range in rangeList)
                {
                    List<int> adressInt = ipAddress.Split('.').Select(str => int.Parse(str)).ToList();
                    List<int> lowerInt = range.Lower.ToString().Split('.').Select(str => int.Parse(str)).ToList();
                    List<int> upperInt = range.Upper.ToString().Split('.').Select(str => int.Parse(str)).ToList();

                    if (adressInt[0] >= lowerInt[0] && adressInt[0] < upperInt[0])
                    {
                        return true;
                    }
                    else if (adressInt[0] >= lowerInt[0] && adressInt[0] == upperInt[0])
                    {
                        if (adressInt[1] >= lowerInt[1] && adressInt[1] < upperInt[1])
                        {
                            return true;
                        }
                        else if (adressInt[1] >= lowerInt[1] && adressInt[1] == upperInt[1])
                        {
                            if (adressInt[2] >= lowerInt[2] && adressInt[2] < upperInt[2])
                            {
                                return true;
                            }
                            else if (adressInt[2] >= lowerInt[2] && adressInt[2] == upperInt[2])
                            {
                                if (adressInt[3] >= lowerInt[3] && adressInt[3] <= upperInt[3])
                                {
                                    return true;
                                }
                            }

                        }

                    }
                }*/
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}