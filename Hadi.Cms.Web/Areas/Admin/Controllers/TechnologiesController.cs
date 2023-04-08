using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class TechnologiesController : Controller
    {
        private readonly TechnologyService _technologyService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ProjectTechnologyService _projectTechnologyService;

        public TechnologiesController()
        {
            _technologyService = new TechnologyService();
            _eventLoger = new EventLoger();
            _attachmentFileService = new AttachmentFileService();
            _projectTechnologyService = new ProjectTechnologyService();
        }

        /// <summary>
        /// دریافت لیست تکنولوژی ها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var technologies = _technologyService.GetList(t => !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id , t => t.OrderByDescending(o => o.CreatedDate));

            foreach (var item in technologies)
                item.ImageSource = item.ImageGuid.HasValue ? _attachmentFileService.GetAttachmentSourceValue(item.ImageGuid) : "/PhysicalStorage/no_image.png";

            var pagedList = technologies.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت تکنولوژِی
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new TechnologyCreateCommand());
        }

        /// <summary>
        /// ثبت تکنولوژی جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TechnologyCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            #region Insert image in attachment file
            if (file != null && file.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));

                var imageHeight = 0;
                var imageWidth = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "TechnologyImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageGuid = attachmentId;
            }
            #endregion

            _technologyService.CreateNewTechnology(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TechnologiesController", "Create", "Success Create Technology", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش تکنولوژی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var technology = _technologyService.Get(id);
            if (technology == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            return View(new TechnologyEditCommand
            {
                Id = technology.Id,
                Name = technology.Name,
                ImageGuid = technology.ImageGuid,
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(technology.ImageGuid),
                IsActive = technology.IsActive,
                IsTool = technology.IsTool
            });
        }

        /// <summary>
        /// ویرایش تکنولوِژِی
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TechnologyEditCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var technology = _technologyService.Get(t => t.Id == command.Id).MapToEntity();
            if (technology == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            #region Insert image in attachment file
            if (file != null && file.ContentLength > 0)
            {
                if (technology.ImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(technology.ImageGuid.Value);

                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }

                var imageWidth = 0;
                var imageHeight = 0;
                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));

                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "TechnologyImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageGuid = attachmentId;
            }
            #endregion

            _technologyService.UpdateTechnology(technology, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TechnologiesController", "Edit", "Success Edit Technology", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var technology = _technologyService.Get(id).MapToEntity();
            if (technology == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            technology.IsActive = !technology.IsActive;
            _technologyService.Update(technology);
            _technologyService.Save();
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// حذف تکنولوژی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var technology = _technologyService.Get(t => t.Id == id).MapToEntity();

            #region Remove dependencies

            var projectTechnologies = _projectTechnologyService.GetList(pt => pt.TechnologyId == technology.Id)
                .MapToEntities();
            foreach (var projectTechnology in projectTechnologies)
            {
                _projectTechnologyService.Delete(projectTechnology.Id);
            }
            _projectTechnologyService.Save();

            #endregion

            _technologyService.Delete(technology.Id);
            _technologyService.Save();
            return Json(new
            {
                Message = Strings.Technology_Delete_Successfully,
                Success = Strings.Success,
                Type = "success"
            });
        }
    }
}