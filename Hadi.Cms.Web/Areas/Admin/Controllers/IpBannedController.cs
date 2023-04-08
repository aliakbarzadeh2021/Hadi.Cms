using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Repository.UnitOfWork;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class IpBannedController : BaseController
    {

        private IpBannedService _ipBannedService;
        private EventLoger _eventLoger;

        public IpBannedController()
        {
            _ipBannedService = new IpBannedService();
            _eventLoger = new EventLoger();
        }

        public ActionResult List(int pageNumber = 1)
        {
            var pageSize = 10;
            var ipBanneds = _ipBannedService.GetList(null , o => o.OrderByDescending(i => i.CreateDate));
            var pagedList = ipBanneds.ToPagedList(pageNumber , pageSize);
            ViewBag.PageNumber = pageNumber;
            return View(pagedList);
        }


        public ActionResult Create()
        {
            return View(new IpBannedModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IpBannedModel model)
        {
            if (ModelState.IsValid)
            {
                var existsIP = _ipBannedService.Any(i => i.IpAddress == model.IpAddress);
                if (existsIP)
                {
                    ModelState.AddModelError("IpAddress", Strings.IP_Exists);
                    return View(model);
                }
                var newIpBanned = new IpBanned()
                {
                    CreatedBy = SessionData.Current.User.Id,
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    IpAddress = model.IpAddress,
                    IpAddressBanReason = model.IpAddressBanReason
                };

                _ipBannedService.Insert(newIpBanned);
                _ipBannedService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "IpBannedController", "Create", "Success Create IpBanned", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                ViewBag.Message = Strings.IP_Banned_Successfully_Create;
                ViewBag.Success = "1";
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteIpBanned(Guid IpBannedId)
        {
            try
            {
                using (DataContext dataContext = new DataContext())
                {
                    var item = _ipBannedService.Get(q => q.Id == IpBannedId);
                    if (item != null)
                    {
                        _ipBannedService.DeleteById(item.Id);
                        _ipBannedService.Save();

                        ViewBag.Message = Strings.IP_Banned_Successfully_Deleted;
                        ViewBag.Success =Strings.Success;
                    }
                    else
                    {
                        ViewBag.Message = Strings.IP_Banned_failed_Deleted_Permission;
                        ViewBag.Success = Strings.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.IP_Banned_Falied_Deleted;
                ViewBag.Success = Strings.Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(Guid IpId)
        {
            var item = _ipBannedService.Get(q => q.Id == IpId).MaptoEntity();
            if (item != null)
            {
                item.IsActive = item.IsActive ? false : true;
                _ipBannedService.Update(item);
                _ipBannedService.Save();
            }
            return RedirectToAction("List");
        }
    }
}