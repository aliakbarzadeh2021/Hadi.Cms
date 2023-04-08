using Hadi.Cms.ApplicationService.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// دلایل انتخاب دپارتمان
    /// </summary>
    public class DepartmentSelectionReasonController : Controller
    {
        private readonly DepartmentSelectionReasonService _departmentSelectionReasonService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public DepartmentSelectionReasonController()
        {
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
            _departmentSelectionReasonService = new DepartmentSelectionReasonService();
        }

        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var departmentSelectionReasons =
                _departmentSelectionReasonService.GetList(d =>
                    !d.IsDeleted && d.DepartmentId == id && d.CreatedBy == SessionData.Current.User.Id);

            foreach (var departmentSelectionReason in departmentSelectionReasons)
                departmentSelectionReason.ImageSource = departmentSelectionReason.AttachmentImageId.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(departmentSelectionReason.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = departmentSelectionReasons.OrderByDescending(d => d.CreatedDate)
                .ToPagedList(pageNumber, pageSize);

            ViewBag.DepartmentId = id;
            ViewBag.PageNumber = pageNumber;
            
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");
            TempData["Id"] = id;
            TempData.Keep("Id");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت
        /// </summary>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            return View(new DepartmentSelectionReasonCreateCommand
            {
                DepartmentId = id
            });
        }

        /// <summary>
        /// ثبت جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentSelectionReasonCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var existsDepartmentSelectionReason = _departmentSelectionReasonService.Any(d => d.Title == command.Title);
            if (existsDepartmentSelectionReason)
            {
                ModelState.AddModelError("Title", Strings.DepartmentSelectionReason_Exists_Title);
                return View(command);
            }

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
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "DepartmentSelectionReasonImage - " + Guid.NewGuid(), "", imageData,
                    DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            #endregion

            _departmentSelectionReasonService.CreateNewDepartmentSelectionReason(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName,
                "DepartmentSelectionReasonController", "Create", "Success Create DepartmentSelectionReason",
                HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = command.DepartmentId, pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id, Guid departmentId)
        {
            var departmentSelectionReason = _departmentSelectionReasonService.Get(id);
            if (departmentSelectionReason == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new DepartmentSelectionReasonEditCommand
            {
                Id = departmentSelectionReason.Id,
                DepartmentId = departmentSelectionReason.DepartmentId,
                Title = departmentSelectionReason.Title,
                Description = departmentSelectionReason.Description?.Replace("<br/>", "\r\n"),
                AttachmentImageId = departmentSelectionReason.AttachmentImageId,
                ImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(departmentSelectionReason.AttachmentImageId),
                IsActive = departmentSelectionReason.IsActive
            });
        }

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentSelectionReasonEditCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var departmentSelectionReason = _departmentSelectionReasonService.Get(command.Id).MapToEntity();
            if (departmentSelectionReason == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            var existsDepartmentSelectionReason = _departmentSelectionReasonService.Any(d => d.Title == command.Title && d.Id != command.Id);
            if (existsDepartmentSelectionReason)
            {
                ModelState.AddModelError("Title", Strings.DepartmentSelectionReason_Exists_Title);
                return View(command);
            }

            #region Insert image in attachment file

            if (file != null && file.ContentLength > 0)
            {
                if (command.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(command.AttachmentImageId.Value);

                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "DepartmentSelectionReasonImage - " + Guid.NewGuid(), "", imageData,
                    DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            #endregion

            _departmentSelectionReasonService.UpdateDepartmentSelectionReason(departmentSelectionReason, command,
                SessionData.Current.User.Id);

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName,
                "DepartmentSelectionReasonController", "Edit", "Success Edit DepartmentSelectionReason",
                HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id, Guid departmentId)
        {
            var departmentSelectionReason = _departmentSelectionReasonService.Get(id).MapToEntity();
            if (departmentSelectionReason == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });


            departmentSelectionReason.IsActive = !departmentSelectionReason.IsActive;
            _departmentSelectionReasonService.Update(departmentSelectionReason);
            _departmentSelectionReasonService.Save();
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

        }

        /// <summary>
        /// حذف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var departmentSelectionReason = _departmentSelectionReasonService.Get(id).MapToEntity();

                #region Remove attachment
                if (departmentSelectionReason.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(departmentSelectionReason.AttachmentImageId.Value);
                #endregion

                _departmentSelectionReasonService.Delete(departmentSelectionReason.Id);
                _departmentSelectionReasonService.Save();
                return Json(new
                {
                    Message = Strings.DepartmentSelectionReason_Delete_Successfully,
                    Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Strings.Success,
                    Type = "error"
                });
            }
        }
    }
}