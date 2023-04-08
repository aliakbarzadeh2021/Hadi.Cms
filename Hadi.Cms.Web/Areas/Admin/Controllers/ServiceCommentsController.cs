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
    /// <summary>
    /// مدیریت کامنت های خدممت
    /// </summary>
    public class ServiceCommentsController : Controller
    {
        private readonly ServiceCommentService _serviceCommentService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public ServiceCommentsController()
        {
            _serviceCommentService = new ServiceCommentService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست کامنت های خدمت
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var serviceComments = _serviceCommentService.GetList(s => s.ServiceId == id && !s.IsDeleted);

            foreach (var serviceComment in serviceComments)
                serviceComment.AttachmentImageSource = serviceComment.AttachmentImageId.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(serviceComment.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = serviceComments.OrderByDescending(s => s.CreatedDate).ToPagedList(pageNumber, pageSize);
            ViewBag.ServiceId = id;

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت کامنت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            ViewBag.ServiceId = id;

            return View(new ServiceCommentCreateCommand()
            {
                ServiceId = id
            });
        }

        /// <summary>
        /// ثبت کامنت
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceCommentCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            #region Insert image in attachment file .

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

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "ServiceCommentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            _serviceCommentService.CreateNewServiceComment(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ServiceCommentsController", "Create", "Success Create ServiceComment", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent); _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "DepartmentsController", "Create", "Success Create Department", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = command.ServiceId, pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش کامنت
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id, Guid serviceId)
        {
            var serviceComment = _serviceCommentService.Get(id);
            if (serviceComment == null)
                return RedirectToAction("Index", new { id = serviceId, pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new ServiceCommentEditCommand()
            {
                Id = id,
                ServiceId = serviceId,
                PersonFullName = serviceComment.PersonFullName,
                PersonRoleTitle = serviceComment.PersonRoleTitle,
                AttachmentImageId = serviceComment.AttachmentImageId,
                AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(serviceComment.AttachmentImageId),
                Text = serviceComment.Text
            });
        }

        /// <summary>
        /// ویرایش کامنت
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceCommentEditCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var serviceComment = _serviceCommentService.Get(command.Id).MapToEntity();
            if (serviceComment == null)
                return RedirectToAction("Index", new { id = command.ServiceId, pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Insert image in attachment file .

            if (file != null && file.ContentLength > 0)
            {
                if (serviceComment.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(serviceComment.AttachmentImageId.Value);

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

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "ServiceCommentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            _serviceCommentService.UpdateServiceComment(serviceComment, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ServiceCommentsController", "Create", "Success Create ServiceComment", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent); _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "DepartmentsController", "Edit", "Success Edit Department", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = command.ServiceId, pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }


        /// <summary>
        /// حذف کامنت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var serviceComment = _serviceCommentService.Get(id);
                _serviceCommentService.Delete(serviceComment.Id);
                _serviceCommentService.Save();
                return Json(new
                {
                    Message = Strings.ServiceComment_Delete_Successfully,
                    Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Strings.Error,
                    Type = "error"
                });
            }
        }
    }
}