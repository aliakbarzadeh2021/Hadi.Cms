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
    /// مدیریت خبرهای سامانه
    /// </summary>
    public class NlMessagesController : Controller
    {
        private readonly NlMessageService _nlMessageService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly NlMessageEmailService _nlMessageEmailService;
        private readonly NlEmailService _nlEmailService;
        private readonly EventLoger _eventLoger;

        public NlMessagesController()
        {
            _nlMessageService = new NlMessageService();
            _attachmentFileService = new AttachmentFileService();
            _nlMessageEmailService = new NlMessageEmailService();
            _nlEmailService = new NlEmailService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست خبرها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var nlMessages = _nlMessageService.GetList(n => !n.IsDeleted, o => o.OrderByDescending(n => n.CreatedDate));
            foreach (var nlMessage in nlMessages)
            {
                nlMessage.AttachmentSource = _attachmentFileService.GetAttachmentSourceValue(nlMessage.AttachmentId);
            }
            var pagedList = nlMessages.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت خبر جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new NlMessageCreateCommand());
        }

        /// <summary>
        /// ثبت خبر
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NlMessageCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            if (file != null && file.ContentLength > 0)
            {
                byte[] fileDate;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    fileDate = binaryReader.ReadBytes(file.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(fileDate));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                    System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "NlMessageFile - " + Guid.NewGuid(), "", fileDate, DateTime.Now);

                command.AttachmentId = attachmentId;
            }
            var newNlMessageId = _nlMessageService.CreateNewMessage(command, SessionData.Current.User.Id);

            if (newNlMessageId != Guid.Empty)
            {
                var nlEmailIds = _nlEmailService.GetList().Select(n => n.Id).ToList();
                if (nlEmailIds.Count > 0)
                {
                    _nlMessageEmailService.AssignMessageToEmailWithPublishedFalse(newNlMessageId, nlEmailIds,
                                SessionData.Current.User.Id);
                }
            }

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "NlMessagesController", "Create", "Success Create NlMessage", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// حذف خبر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var nlMessage = _nlMessageService.Get(id);

            #region Remove dependencies

            var nlEmailMessages = _nlMessageEmailService.GetList(n => n.NlMessageId == nlMessage.Id);
            foreach (var item in nlEmailMessages)
            {
                _nlMessageEmailService.Delete(item.Id);
            }
            _nlMessageEmailService.Save();

            #endregion

            _nlMessageService.Delete(nlMessage.Id);
            _nlMessageService.Save();


            return Json(new
            {
                Message = Strings.NlMessage_Delete_Successfully,
                Strings.Success
            });
        }
    }
}