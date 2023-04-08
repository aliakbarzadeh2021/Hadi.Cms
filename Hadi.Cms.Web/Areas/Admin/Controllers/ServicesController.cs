using System;
using System.Collections.Generic;
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
    /// مدیریت خدمات
    /// </summary>
    public class ServicesController : Controller
    {
        private readonly ServiceService _serviceService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly DepartmentServiceService _departmentServiceService;
        private readonly ServiceTagService _serviceTagService;
        private readonly TagService _tagService;
        private readonly EventLoger _eventLoger;
        private readonly ServiceCommentService _serviceCommentService;

        public ServicesController()
        {
            _serviceService = new ServiceService();
            _attachmentFileService = new AttachmentFileService();
            _departmentServiceService = new DepartmentServiceService();
            _serviceTagService = new ServiceTagService();
            _tagService = new TagService();
            _eventLoger = new EventLoger();
            _serviceCommentService = new ServiceCommentService();
        }

        /// <summary>
        /// دریافت لیست خدمات
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var services = _serviceService.GetList(s => !s.IsDeleted && s.CreatedBy == SessionData.Current.User.Id);

            foreach (var service in services)
                service.AttachmentImageSource = service.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(service.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = services.OrderByDescending(s => s.CreatedDate).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت خدمت جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Tags =
                _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);

            return View(new ServiceCreateCommand());
        }

        /// <summary>
        /// ثبت خدمت جدید
        /// </summary>
        /// <param name="command"></param>
        /// /// <param name="logoService"></param>
        /// <param name="file"></param>
        /// <param name="sectionOneThumbnailImage"></param>
        /// <param name="sectionOneImage"></param>
        /// <param name="sectionTwoThumbnailImage"></param>
        /// <param name="sectionTwoImage"></param>
        /// <param name="sectionThreeThumbnailImage"></param>
        /// <param name="sectionThreeImage"></param>
        /// <param name="sectionFourThumbnailImage"></param>
        /// <param name="sectionFourImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceCreateCommand command, HttpPostedFileBase logoService, HttpPostedFileBase file,
            HttpPostedFileBase sectionOneThumbnailImage, HttpPostedFileBase sectionOneImage,
            HttpPostedFileBase sectionTwoThumbnailImage, HttpPostedFileBase sectionTwoImage,
            HttpPostedFileBase sectionThreeThumbnailImage, HttpPostedFileBase sectionThreeImage,
            HttpPostedFileBase sectionFourThumbnailImage, HttpPostedFileBase sectionFourImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tags =
                    _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);
                return View(command);
            }

            var existsService = _serviceService.Any(s => s.Title == command.Title);
            if (existsService)
            {
                ViewBag.Tags =
                _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);

                ModelState.AddModelError("Title", Strings.Service_Exists_Title);
                return View(command);
            }

            #region Insert image in attachment file

            #region Service logo image

            if (logoService != null && logoService.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = logoService.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(logoService.InputStream))
                {
                    imageData = binaryReader.ReadBytes(logoService.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(logoService.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(logoService.FileName),
                    System.IO.Path.GetExtension(logoService.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "ServiceLogoImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ServiceLogoId = attachmentId;
            }
            #endregion

            #region Service image .
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
                    imageWidth, imageHeight, "ServiceImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            #region Section one thumbnail image
            if (sectionOneThumbnailImage != null && sectionOneThumbnailImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionOneThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionOneThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionOneThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionOneThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionOneThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionOneThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionOneThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionOneThumbnailImageGuid = attachmentId;
            }
            #endregion

            #region Section one image

            if (sectionOneImage != null && sectionOneImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionOneImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionOneImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionOneImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionOneImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionOneImage.FileName),
                    System.IO.Path.GetExtension(sectionOneImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionOneImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionOneImageGuid = attachmentId;
            }
            #endregion

            #region Section two thumbnail image

            if (sectionTwoThumbnailImage != null && sectionTwoThumbnailImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionTwoThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionTwoThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionTwoThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionTwoThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionTwoThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionTwoThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionTwoThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionTwoThumbnailImageGuid = attachmentId;
            }
            #endregion

            #region Section two image

            if (sectionTwoImage != null && sectionTwoImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionTwoImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionTwoImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionTwoImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionTwoImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionTwoImage.FileName),
                    System.IO.Path.GetExtension(sectionTwoImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionTwoImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionTwoImageGuid = attachmentId;
            }

            #endregion

            #region Section three thumbnail image

            if (sectionThreeThumbnailImage != null && sectionThreeThumbnailImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionThreeThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionThreeThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionThreeThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionThreeThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionThreeThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionThreeThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionThreeThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionThreeThumbnailImageGuid = attachmentId;
            }

            #endregion

            #region Section three image

            if (sectionThreeImage != null && sectionThreeImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionThreeImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionThreeImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionThreeImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionThreeImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionThreeImage.FileName),
                    System.IO.Path.GetExtension(sectionThreeImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionThreeImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionThreeImageGuid = attachmentId;
            }

            #endregion

            #region Section four thumbnail image

            if (sectionFourThumbnailImage != null && sectionFourThumbnailImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionFourThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionFourThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionFourThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionFourThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionFourThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionFourThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionFourThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionFourThumbnailImageGuid = attachmentId;
            }
            #endregion

            #region Section four image

            if (sectionFourImage != null && sectionFourImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sectionFourImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionFourImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionFourImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionFourImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionFourImage.FileName),
                    System.IO.Path.GetExtension(sectionFourImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionFourImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionFourImageGuid = attachmentId;
            }

            #endregion

            #endregion

            var newServiceId = _serviceService.CreateNewService(command, SessionData.Current.User.Id);

            #region Assign tags to service .

            command.ServiceTagsId?.RemoveAll(s => s == Guid.Empty);
            _serviceTagService.AssignTagsToService(newServiceId, command.ServiceTagsId, SessionData.Current.User.Id);
            #endregion


            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ServicesController", "Create", "Success Create Service", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش خدمت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var service = _serviceService.GetList(s => s.Id == id, null, s => s.ServiceTags).FirstOrDefault();
            if (service == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Fill tags

            ViewBag.Tags =
                _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);

            List<string> tagList = new List<string>();
            foreach (var item in service.ServiceTagDto)
            {
                tagList.Add(item.TagId.ToString());
            }
            ViewBag.SelectedTags = string.Join(",", tagList);
            #endregion


            return View(new ServiceEditCommand
            {
                Id = service.Id,
                Title = service.Title?.Replace("<br/>","\r\n"),
                Description = service.Description,
                ServiceLogoSource = _attachmentFileService.GetAttachmentSourceValue(service.ServiceLogoId),
                ServiceLogoId = service.ServiceLogoId,
                SectionOneDescription = service.SectionOneDescription,
                SectionOneImageGuid = service.SectionOneImageGuid,
                SectionOneImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionOneImageGuid),
                SectionOneThumbnailImageGuid = service.SectionOneThumbnailImageGuid,
                SectionOneThumbnailImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionOneThumbnailImageGuid),
                SectionTwoDescription = service.SectionTwoDescription,
                SectionTwoImageGuid = service.SectionTwoImageGuid,
                SectionTwoImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionTwoImageGuid),
                SectionTwoThumbnailImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionTwoThumbnailImageGuid),
                SectionTwoThumbnailImageGuid = service.SectionTwoThumbnailImageGuid,
                SectionThreeDescription = service.SectionThreeDescription,
                SectionThreeImageGuid = service.SectionThreeImageGuid,
                SectionThreeImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionThreeImageGuid),
                SectionThreeThumbnailImageGuid = service.SectionThreeThumbnailImageGuid,
                SectionThreeThumbnailImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionThreeThumbnailImageGuid),
                SectionFourDescription = service.SectionFourDescription,
                SectionFourImageGuid = service.SectionFourImageGuid,
                SectionFourImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionFourImageGuid),
                SectionFourThumbnailImageGuid = service.SectionFourThumbnailImageGuid,
                SectionFourThumbnailImageSource = _attachmentFileService.GetAttachmentSourceValue(service.SectionFourThumbnailImageGuid),
                AttachmentImageId = service.AttachmentImageId,
                Source = _attachmentFileService.GetAttachmentSourceValue(service.AttachmentImageId),
                IsActive = service.IsActive
            });
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        /// <param name="command"></param>
        /// <param name="logoService"></param>
        /// <param name="file"></param>
        /// <param name="sectionOneThumbnailImage"></param>
        /// <param name="sectionOneImage"></param>
        /// <param name="sectionTwoThumbnailImage"></param>
        /// <param name="sectionTwoImage"></param>
        /// <param name="sectionThreeThumbnailImage"></param>
        /// <param name="sectionThreeImage"></param>
        /// <param name="sectionFourThumbnailImage"></param>
        /// <param name="sectionFourImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceEditCommand command, HttpPostedFileBase logoService, HttpPostedFileBase file,
            HttpPostedFileBase sectionOneThumbnailImage, HttpPostedFileBase sectionOneImage,
            HttpPostedFileBase sectionTwoThumbnailImage, HttpPostedFileBase sectionTwoImage,
            HttpPostedFileBase sectionThreeThumbnailImage, HttpPostedFileBase sectionThreeImage,
            HttpPostedFileBase sectionFourThumbnailImage, HttpPostedFileBase sectionFourImage)
        {
            var service = _serviceService.GetList(s => s.Id == command.Id, null, s => s.ServiceTags).FirstOrDefault().MapToEntity();
            if (service == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            if (!ModelState.IsValid)
            {
                #region Fill tags

                ViewBag.Tags =
                    _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);

                List<string> tagList = new List<string>();
                foreach (var item in service.ServiceTags)
                {
                    tagList.Add(item.TagId.ToString());
                }
                ViewBag.SelectedTags = string.Join(",", tagList);
                #endregion

                return View(command);
            }

            var existsService = _serviceService.Any(s => s.Title == command.Title && s.Id != service.Id);
            if (existsService)
            {
                ModelState.AddModelError("Title", Strings.Service_Exists_Title);
                return View(command);
            }

            #region Service logo image

            if (logoService != null && logoService.ContentLength > 0)
            {
                if (service.ServiceLogoId.HasValue)
                    _attachmentFileService.RemoveAttachment(service.ServiceLogoId.Value);

                byte[] imageData;
                var fileSize = logoService.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(logoService.InputStream))
                {
                    imageData = binaryReader.ReadBytes(logoService.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(logoService.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(logoService.FileName),
                    System.IO.Path.GetExtension(logoService.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "ServiceLogoImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ServiceLogoId = attachmentId;
            }
            #endregion

            #region Service image .
            if (file != null && file.ContentLength > 0)
            {
                if (service.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(service.AttachmentImageId.Value);

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
                    imageWidth, imageHeight, "ServiceImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            #region Section one thumbnail image
            if (sectionOneThumbnailImage != null && sectionOneThumbnailImage.ContentLength > 0)
            {
                if (service.SectionOneThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionOneThumbnailImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionOneThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionOneThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionOneThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionOneThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionOneThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionOneThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionOneThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionOneThumbnailImageGuid = attachmentId;
            }
            #endregion

            #region Section one image

            if (sectionOneImage != null && sectionOneImage.ContentLength > 0)
            {
                if (service.SectionOneImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionOneImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionOneImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionOneImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionOneImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionOneImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionOneImage.FileName),
                    System.IO.Path.GetExtension(sectionOneImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionOneImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionOneImageGuid = attachmentId;
            }
            #endregion

            #region Section two thumbnail image

            if (sectionTwoThumbnailImage != null && sectionTwoThumbnailImage.ContentLength > 0)
            {
                if (service.SectionTwoThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionTwoThumbnailImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionTwoThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionTwoThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionTwoThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionTwoThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionTwoThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionTwoThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionTwoThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionTwoThumbnailImageGuid = attachmentId;
            }
            #endregion

            #region Section two image

            if (sectionTwoImage != null && sectionTwoImage.ContentLength > 0)
            {
                if (service.SectionTwoImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionTwoImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionTwoImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionTwoImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionTwoImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionTwoImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionTwoImage.FileName),
                    System.IO.Path.GetExtension(sectionTwoImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionTwoImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionTwoImageGuid = attachmentId;
            }

            #endregion

            #region Section three thumbnail image

            if (sectionThreeThumbnailImage != null && sectionThreeThumbnailImage.ContentLength > 0)
            {
                if (service.SectionThreeImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionThreeImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionThreeThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionThreeThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionThreeThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionThreeThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionThreeThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionThreeThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionThreeThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionThreeThumbnailImageGuid = attachmentId;
            }

            #endregion

            #region Section three image

            if (sectionThreeImage != null && sectionThreeImage.ContentLength > 0)
            {
                if (service.SectionThreeImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionThreeImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionThreeImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionThreeImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionThreeImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionThreeImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionThreeImage.FileName),
                    System.IO.Path.GetExtension(sectionThreeImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionThreeImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionThreeImageGuid = attachmentId;
            }

            #endregion

            #region Section four thumbnail image

            if (sectionFourThumbnailImage != null && sectionFourThumbnailImage.ContentLength > 0)
            {
                if (service.SectionFourThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionFourThumbnailImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionFourThumbnailImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionFourThumbnailImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionFourThumbnailImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionFourThumbnailImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionFourThumbnailImage.FileName),
                    System.IO.Path.GetExtension(sectionFourThumbnailImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionFourThumbnailImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionFourThumbnailImageGuid = attachmentId;
            }
            #endregion

            #region Section four image

            if (sectionFourImage != null && sectionFourImage.ContentLength > 0)
            {
                if (service.SectionFourImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionFourImageGuid.Value);

                byte[] imageData;
                var fileSize = sectionFourImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sectionFourImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sectionFourImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sectionFourImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sectionFourImage.FileName),
                    System.IO.Path.GetExtension(sectionFourImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SectionFourImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SectionFourImageGuid = attachmentId;
            }

            #endregion

            #region Assign tags to service .

            command.ServiceTagsId?.RemoveAll(s => s == Guid.Empty);
            _serviceTagService.AssignTagsToService(command.Id, command.ServiceTagsId, SessionData.Current.User.Id);
            #endregion

            _serviceService.UpdateService(service, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ServicesController", "Edit", "Success Edit Service", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت خدمت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var service = _serviceService.Get(id).MapToEntity();
            if (service == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            service.IsActive = !service.IsActive;
            _serviceService.Update(service);
            _serviceService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف خدمت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var service = _serviceService.Get(id).MapToEntity();

                #region Remove dependencies

                if (service.ServiceLogoId.HasValue)
                    _attachmentFileService.GetAttachmentSourceValue(service.ServiceLogoId.Value);

                if (service.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(service.AttachmentImageId.Value);

                if (service.SectionOneImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionOneImageGuid.Value);

                if (service.SectionOneThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionOneThumbnailImageGuid.Value);

                if (service.SectionTwoImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionTwoImageGuid.Value);

                if (service.SectionTwoThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionTwoThumbnailImageGuid.Value);

                if (service.SectionThreeImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionThreeImageGuid.Value);

                if (service.SectionThreeThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionThreeThumbnailImageGuid.Value);

                if (service.SectionFourImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionFourImageGuid.Value);

                if (service.SectionFourThumbnailImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(service.SectionFourThumbnailImageGuid.Value);

                var departmentServices = _departmentServiceService.GetList(d => d.ServiceId == service.Id);
                foreach (var departmentService in departmentServices)
                {
                    _departmentServiceService.Delete(departmentService.Id);
                }
                _departmentServiceService.Save();

                var serviceTags = _serviceTagService.GetList(s => s.ServiceId == service.Id);
                foreach (var serviceTag in serviceTags)
                {
                    _serviceTagService.Delete(serviceTag.Id);
                }
                _serviceTagService.Save();

                var serviceComments = _serviceCommentService.GetList(s => s.ServiceId == service.Id);
                foreach (var serviceComment in serviceComments)
                {
                    _serviceCommentService.Delete(serviceComment.Id);
                }
                _serviceCommentService.Save();

                #endregion

                _serviceService.Delete(service.Id);
                _serviceService.Save();
                return Json(new
                {
                    Message = Strings.Service_Success_Delete,
                    Success = Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error,
                    Type = "error"
                });
            }
        }
    }
}