using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class IpRangeController : BaseController
    {
        private IpRangeService _ipRangeService;
        private EventLoger _eventLoger;

        public IpRangeController()
        {
            _ipRangeService = new IpRangeService();
            _eventLoger = new EventLoger();
        }

        public ActionResult List(int pageNumber = 1)
        {
            var pageSize = 10;
            var ipRanges = _ipRangeService.GetList(null , o => o.OrderByDescending(i => i.CreateDate));
            var pagedList = ipRanges.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

        public ActionResult Create()
        {
            return View(new IpRangeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IpRangeModel model)
        {
            if (ModelState.IsValid)
            {
                var existsIP = _ipRangeService.Any(i => i.Lower == model.Lower.ToLower() || i.Upper == model.Upper.ToUpper());
                if (existsIP)
                {
                    ModelState.AddModelError("Lower", Strings.IP_Exists);
                    return View(model);
                }
                var newIpRange = new IpRange()
                {
                    CreatedBy = SessionData.Current.User.Id,
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    Lower = model.Lower,
                    Upper = model.Upper
                };

                _ipRangeService.Insert(newIpRange);
                _ipRangeService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "IpRangeController", "Create", "Success Create IpRange", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                ViewBag.Message = Strings.New_Ip_Created;
                ViewBag.Success = "1";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteIpRange(Guid IpRangeId)
        {
            try
            {
                var item = _ipRangeService.Get(q => q.Id == IpRangeId);
                if (item != null)
                {
                    _ipRangeService.DeleteById(item.Id);
                    _ipRangeService.Save();

                    ViewBag.Message = Strings.Delete_Ip_Range_Success;
                    ViewBag.Success = Strings.Is_Deleted;
                }
                else
                {
                    ViewBag.Message = Strings.Delete_Ip_Range_Error;
                    ViewBag.Success = Strings.Is_Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.Delete_Ip_Range_Error;
                ViewBag.Success = Strings.Is_Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(Guid IpId)
        {
            var item = _ipRangeService.Get(q => q.Id == IpId).MaptoEntity();
            if (item != null)
            {
                item.IsActive = item.IsActive ? false : true;
                _ipRangeService.Update(item);
                _ipRangeService.Save();
            }
            return RedirectToAction("List");
        }
    }
}