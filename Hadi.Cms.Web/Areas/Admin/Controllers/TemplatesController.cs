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
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class TemplatesController : BaseController
    {
        private TemplateService _templateService;
        private EventLoger _eventLoger;

        public TemplatesController()
        {
            _templateService = new TemplateService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;

            var templates = _templateService.GetList(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id).OrderByDescending(q => q.CreatedDate).ToList();

            var pagedListTemplates = templates.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedListTemplates);
        }


        public ActionResult Create()
        {
            return View(new TemplateModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TemplateModel model)
        {
            if (ModelState.IsValid)
            {
                Encoding utf8 = Encoding.UTF8;
                byte[] encodedBytes = utf8.GetBytes(model.Source);
                string decodedString = utf8.GetString(encodedBytes);

                Template template = new Template
                {
                        
                    Title = model.Title,
                    Source = decodedString,
                    IsActive = model.IsActive,
                    IsDeleted = false,
                    CreatedBy = SessionData.Current.User.Id
                };
                _templateService.Insert(template);
                _templateService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TemplatesController", "Create", "Success Create Template", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            var template = _templateService.Get(q => !q.IsDeleted && q.Id == id && q.CreatedBy == SessionData.Current.User.Id);

            var templateModel = new TemplateModel
            {
                CreatedDate = template.CreatedDate,
                CreatedBy = template.CreatedBy,
                IsActive = template.IsActive,
                IsDeleted = template.IsDeleted,
                ModifiedBy = template.ModifiedBy,
                ModifiedDate = template.ModifiedDate,
                Source = template.Source,
                Id = template.Id,
                Title = template.Title
            };
            return View(templateModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TemplateModel model)
        {
            if (ModelState.IsValid)
            {
                var template = _templateService.Get(q => !q.IsDeleted && q.Id == model.Id && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();

                Encoding utf8 = Encoding.UTF8;
                byte[] encodedBytes = utf8.GetBytes(model.Source);
                string decodedString = utf8.GetString(encodedBytes);

                template.Title = model.Title;
                template.IsActive = model.IsActive;
                template.Source = decodedString;
                template.ModifiedBy = SessionData.Current.User.Id;
                template.ModifiedDate = DateTime.Now;

                _templateService.Update(template);
                _templateService.Save();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult ChangeStatus(Guid templateId)
        {
            var template = _templateService.GetById(templateId).MaptoEntity();
            if (template != null)
            {
                template.IsActive = !template.IsActive;
                _templateService.Update(template);
                _templateService.Save();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                var template = _templateService.Get(q => !q.IsDeleted && q.Id == Id && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();
                if (template != null)
                {
                    template.IsDeleted = true;
                    _templateService.Update(template);
                    _templateService.Save();

                    ViewBag.Message = Strings.Template_Delete_Successfully;
                    ViewBag.Success = Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.Template_Delete_Dont_Have_Permission;
                    ViewBag.Success = Strings.Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.Template_Delete_Error;
                ViewBag.Success = Strings.Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }


    }
}