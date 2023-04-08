using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class EventLogsController : BaseController
    {
        private EventLogService _eventLogService;

        public EventLogsController()
        {
            _eventLogService = new EventLogService();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;

            var eventLogs = _eventLogService.GetList().OrderByDescending(q => q.EventTime).ToList();

            var pagedListEventLogs = eventLogs.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedListEventLogs);
        }

        public ActionResult EventLogListPartial(List<IEventLogDto> model)
        {
            return PartialView(model);
        }

        public ActionResult Details(Guid Id)
        {
            var eventLog = _eventLogService.GetById(Id);

            return View(eventLog);
        }


        public ActionResult ChangeErrorType(string ErrorType, int? pageNumber)
        {
            int page = pageNumber ?? 1;
            int pageSize = 5;

            string type = ErrorType.Substring(0, 1).Trim().ToLower();
            var eventLogs = _eventLogService.GetList(q => q.EventType.Trim().ToLower() == type);

            var pagedListEventLogs = eventLogs.ToPagedList(page, pageSize);

            return PartialView("EventLogListPartial", pagedListEventLogs);
        }

        [HttpPost]
        public ActionResult Delete(Guid eventLogId)
        {
            _eventLogService.DeleteById(eventLogId);
            _eventLogService.Save();
            ViewBag.Message = Strings.Event_Log_Successfully_Removed;
            ViewBag.Success = Strings.Success;
            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

    }
}