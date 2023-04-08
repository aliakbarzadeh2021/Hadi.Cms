using PagedList;
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
using System.Text;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class TemplateDetailsController : BaseController
    {

        private TemplateDetailService _templateDetailService;
        private TemplateService _templateService;
        private EventLoger _eventLoger;

        public TemplateDetailsController()
        {
            _templateDetailService = new TemplateDetailService();
            _templateService = new TemplateService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index(Guid TemplateId, int pageNumber = 1)
        {
            var pageSize = 5;

            var template = _templateService.Get(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id && q.Id == TemplateId);
            return View(new TemplateDetailsListModel
            {
                TemplateDetails = template.TemplateDetailsDto.Where
                (q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id).ToPagedList(pageNumber, pageSize),

                TemplateId = template.Id,
                TemplateTitle = template.Title
            });
        }


        public ActionResult Create(Guid TemplateId)
        {
            return View(new TemplateDetailModel
            {
                TemplateId = TemplateId
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TemplateDetailModel model)
        {
            if (ModelState.IsValid)
            {
                Encoding utf8 = Encoding.UTF8;
                byte[] encodedBytes = utf8.GetBytes(model.Source);
                string decodedString = utf8.GetString(encodedBytes);

                TemplateDetail templateDetail = new TemplateDetail
                {
                    Title = model.Title,
                    Source = decodedString,
                    IsActive = model.IsActive,
                    IsDeleted = false,
                    CreatedBy = SessionData.Current.User.Id,
                    TemplateId = model.TemplateId
                };
                _templateDetailService.Insert(templateDetail);
                _templateDetailService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TemplateDetailsController", "Create", "Success Create TemplateDetail", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                return RedirectToAction("Index", new { TemplateId = model.TemplateId });
            }
            return View(model);
        }

        public ActionResult Edit(Guid id, Guid TemplateId)
        {
            var templateDetail = _templateDetailService.Get(q => !q.IsDeleted && q.Id == id && q.CreatedBy == SessionData.Current.User.Id && q.TemplateId == TemplateId);

            var templateDetailModel = new TemplateDetailModel
            {
                CreatedDate = templateDetail.CreatedDate,
                CreatedBy = templateDetail.CreatedBy,
                IsActive = templateDetail.IsActive,
                IsDeleted = templateDetail.IsDeleted,
                ModifiedBy = templateDetail.ModifiedBy,
                ModifiedDate = templateDetail.ModifiedDate,
                Source = templateDetail.Source,
                Id = templateDetail.Id,
                Title = templateDetail.Title,
                TemplateId = templateDetail.TemplateId
            };

            return View(templateDetailModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TemplateDetailModel model)
        {
            if (ModelState.IsValid)
            {
                var templateDetail = _templateDetailService.Get(q => q.Id == model.Id && !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id && q.TemplateId == model.TemplateId).MaptoEntity();

                Encoding utf8 = Encoding.UTF8;
                byte[] encodedBytes = utf8.GetBytes(model.Source);
                string decodedString = utf8.GetString(encodedBytes);

                templateDetail.Title = model.Title;
                templateDetail.IsActive = model.IsActive;
                templateDetail.Source = decodedString;
                templateDetail.ModifiedBy = SessionData.Current.User.Id;
                templateDetail.ModifiedDate = DateTime.Now;
                templateDetail.TemplateId = model.TemplateId;

                _templateDetailService.Update(templateDetail);
                _templateDetailService.Save();

                return RedirectToAction("Index", new { TemplateId = model.TemplateId });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, Guid TemplateId)
        {
            try
            {
                var templateDetail = _templateDetailService.Get(q => q.Id == Id && !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id && q.TemplateId == TemplateId).MaptoEntity();
                if (templateDetail != null)
                {
                    templateDetail.IsDeleted = true;
                    _templateDetailService.Update(templateDetail);
                    _templateDetailService.Save();

                    ViewBag.Message = " اطلاعات قالب انتخاب شده با موفقیت حذف شد.";
                    ViewBag.Success = "حذف شد!";
                }
                else
                {
                    ViewBag.Message = "شما مجوز حذف این اطلاعات قالب را ندارید.";
                    ViewBag.Success = "خطا!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "خطا در حذف اطلاعات قالب ";
                ViewBag.Success = "خطا!";
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

    }
}