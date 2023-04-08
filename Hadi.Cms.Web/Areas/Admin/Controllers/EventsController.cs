using Hadi.Cms.Repository.UnitOfWork;
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
    /// مدیریت رویدادها
    /// </summary>
    public class EventsController : Controller
    {
        private readonly EventService _eventService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public EventsController()
        {
            _eventService = new EventService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست رویدادها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var events = _eventService.GetList(e => !e.IsDeleted && e.CreatedBy == SessionData.Current.User.Id, e => e.OrderByDescending(ev => ev.CreatedDate));

            foreach (var eventDto in events)
                eventDto.AttachmentImageSource = eventDto.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(eventDto.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = events.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت رویداد جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new EventCreateCommand());
        }

        /// <summary>
        /// ثبت رویداد جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventCreateCommand command, HttpPostedFileBase eventImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            var existsEventTitle = _eventService.Any(e => !e.IsDeleted && e.Title == command.Title);
            if (existsEventTitle)
            {
                ModelState.AddModelError("Title", Strings.Event_Title_Exists);
                return View(command);
            }

            #region Insert event image in attachment file
            if (eventImage != null && eventImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = eventImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(eventImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(eventImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(eventImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(eventImage.FileName),
                    System.IO.Path.GetExtension(eventImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "EventImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            _eventService.CreateNewEvent(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "EventsController", "Create", "Success Create Event", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش رویداد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var eventDto = _eventService.Get(id);
            if (eventDto == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            return View(new EventEditCommand
            {
                Id = eventDto.Id,
                Title = eventDto.Title,
                Link = eventDto.Link,
                AttachmentImageId = eventDto.AttachmentImageId,
                AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(eventDto.AttachmentImageId),
                IsActive = eventDto.IsActive
            });
        }

        /// <summary>
        /// ویرایش رویداد جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventEditCommand command, HttpPostedFileBase eventImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            var eventDto = _eventService.Get(command.Id).MapToEntity();
            if (eventDto == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            var existsEventTitle =
                _eventService.Any(e => !e.IsDeleted && e.Title == command.Title && e.Id != command.Id);
            if (existsEventTitle)
            {
                ModelState.AddModelError("Title", Strings.Event_Title_Exists);
                return View(command);
            }

            #region Insert event image in attachment file

            if (eventImage != null && eventImage.ContentLength > 0)
            {
                if (eventDto.AttachmentImageId.HasValue)

                    _attachmentFileService.RemoveAttachment(eventDto.AttachmentImageId.Value);
                byte[] imageData;
                var fileSize = eventImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(eventImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(eventImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(eventImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(eventImage.FileName),
                    System.IO.Path.GetExtension(eventImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "EventImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            _eventService.UpdateEvent(eventDto, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "EventsController", "Edit", "Success Edit Event", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن رویداد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var eventDto = _eventService.Get(id).MapToEntity();
            if (eventDto == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            eventDto.IsActive = !eventDto.IsActive;
            _eventService.Update(eventDto);
            _eventService.Save();
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// حذف رویداد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var eventDto = _eventService.Get(id);

            _eventService.Delete(eventDto.Id);
            _eventService.Save();

            return Json(new
            {
                Message = Strings.Event_Delete_Successfully,
                Strings.Success
            });
        }
    }
}