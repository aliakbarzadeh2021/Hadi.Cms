using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Web.Utilities;
using System;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Controllers
{
    public class ErrorController : Controller
    {
        private EventLoger _eventLoger;

        public ErrorController()
        {
            _eventLoger = new EventLoger();
        }

        public ActionResult CheckDatabaseVersion(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult UnauthorizedRequest()
        {
            return View();
        }

        public ActionResult NotAccess()
        {
            return View();
        }

        public ViewResult PageNotFound(string aspxerrorpath)
        {
            //Response.StatusCode = 404;

            _eventLoger.LogException("Error - page Not Found", Response.StatusCode.ToString(), new Exception("Error path :  " + aspxerrorpath + "   --  DateTime : " + DateTime.Now), SessionData.Current.User?.Id, SessionData.Current.User?.UserName, HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return View();
        }
    }
}