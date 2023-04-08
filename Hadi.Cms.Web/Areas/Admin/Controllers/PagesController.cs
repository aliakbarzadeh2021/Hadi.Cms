using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers

{
    public class PagesController : BaseController
    {
        private PageService _pageService;
        private AttachmentFileService _attachmentFileService;
        private LanguageService _languageService;
        private EventLoger _eventLoger;

        public PagesController()
        {
            _pageService = new PageService();
            _attachmentFileService = new AttachmentFileService();
            _languageService = new LanguageService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;

            var pages = _pageService.GetList(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id);

            foreach (var item in pages)
            {
                var language = _languageService.GetById(item.LanguageId);
                item.LanguageName = language != null ? language.Name : "";
            }

            var pagedListPages = pages.OrderByDescending(p => p.CreatedDate).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedListPages);
        }


        public ActionResult Create()
        {
            ViewBag.Languages = _languageService.GetList();
            return View(new PageModel());
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PageModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var page = new Page
                {
                    Accepted = model.Accepted,
                    Alias = model.Alias.ToLower().Replace(" ", "-"),
                    CreatedBy = SessionData.Current.User.Id,
                    CreatedDate = DateTime.Now,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    IsDeleted = false,
                    Keywords = model.Keywords,
                    LanguageId = model.LanguageId,
                    OrderId = model.OrderId,
                    Source = model.Source,
                    Title = model.Title
                };

                if (file != null && file.ContentLength > 0)
                {
                    byte[] imageData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    int imageWidth = img.Width;
                    int imageHeight = img.Height;

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                         System.IO.Path.GetExtension(file.FileName), fileSize, MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName)),
                         imageWidth, imageHeight, "PageImage - " + page.Id, "", imageData, DateTime.Now);

                    page.ImageId = attachmentId;
                }
                _pageService.Insert(page);
                _pageService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "PagesController", "Create", "Success Create Page", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            ViewBag.Languages = _languageService.GetList();
            return View(model);
        }

        public ActionResult Edit(Guid Id)
        {
            var page = _pageService.Get(q => q.Id == Id && !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();
            if (page == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            var pageModel = new PageModel
            {
                Accepted = page.Accepted,
                AcceptedBy = page.AcceptedBy,
                AcceptedWhen = page.AcceptedWhen,
                Alias = page.Alias.ToLower().Replace(" ", "-"),
                CreatedBy = page.CreatedBy,
                CreatedDate = page.CreatedDate,
                Description = page.Description,
                Id = page.Id,
                ImageId = page.ImageId,
                IsActive = page.IsActive,
                IsDeleted = page.IsDeleted,
                Keywords = page.Keywords,
                LanguageId = page.LanguageId,
                ModifiedBy = page.ModifiedBy,
                ModifiedDate = page.ModifiedDate,
                OrderId = page.OrderId,
                PublishFrom = page.PublishFrom,
                PublishTo = page.PublishTo,
                Source = page.Source,
                Title = page.Title,
            };

            ViewBag.Languages = _languageService.GetList();
            return View(pageModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var page = _pageService.Get(q => q.Id == model.Id && !q.IsDeleted
                && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();

                if (file != null && file.ContentLength > 0)
                {
                    if (page.ImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(page.ImageId.Value);

                    byte[] imageData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    int imageWidth = img.Width;
                    int imageHeight = img.Height;

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                         System.IO.Path.GetExtension(file.FileName), fileSize, MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName)),
                         imageWidth, imageHeight, "PageImage - " + model.Id, "", imageData, DateTime.Now);

                    model.ImageId = attachmentId;
                }

                page.Alias = model.Alias.ToLower().Replace(" ", "-");
                page.Description = model.Description;
                page.IsActive = model.IsActive;
                page.IsDeleted = model.IsDeleted;
                page.Keywords = model.Keywords;
                page.LanguageId = model.LanguageId;
                page.OrderId = model.OrderId;
                page.Source = model.Source;
                page.Title = model.Title;
                page.ModifiedDate = DateTime.Now;
                page.ModifiedBy = SessionData.Current.User.Id;
                page.ImageId = model.ImageId;

                _pageService.Update(page);
                _pageService.Save();
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
            }
            ViewBag.Languages = _languageService.GetList();
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                var page = _pageService.Get(q => q.IsDeleted == false && q.Id == Id && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();

                if (page != null)
                {
                    page.IsDeleted = true;
                    page.ModifiedDate = DateTime.Now;
                    page.ModifiedBy = SessionData.Current.User.Id;
                    _pageService.Update(page);
                    _pageService.Save();

                    ViewBag.Message = Strings.Page_Delete_Successfully;
                    ViewBag.Success = Strings.Success;
                    ViewBag.Type = "success";
                }

                else
                {
                    ViewBag.Message = Strings.Page_Delete_Dont_Permission_Error;
                    ViewBag.Success = Strings.Error;
                    ViewBag.Type = "error";
                }
            }

            catch (Exception ex)
            {
                ViewBag.Message = Strings.Page_Delete_Error;
                ViewBag.Success = Strings.Error;
                ViewBag.Type = "error";
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success,
                Type = ViewBag.Type
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChangeStatus(Guid pageId)
        {
            var page = _pageService.GetById(pageId).MaptoEntity();

            if (page != null)
            {
                page.IsActive = !page.IsActive;
                _pageService.Update(page);
                _pageService.Save();
            }

            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

    }
}