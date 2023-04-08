using System.Web;

namespace Hadi.Cms.Web.Utilities
{
    public class IpProvider
    {
        public static string GetIpValue() 
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}