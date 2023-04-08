using Hadi.Cms.Web.Utilities;
using Hadi.Cms.Web.Utilities.Authorization;
using System.Text;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Controllers
{
    [CheckIpBanned]
    [CheckIpRange]
    [CheckUserEnable]
    [CheckChangePassword]
    [MvcAuthorize]
    public abstract class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDotNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}