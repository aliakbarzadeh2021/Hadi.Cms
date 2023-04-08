using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Notification.Email;
using Hadi.Cms.Repository.UnitOfWork;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using Hadi.Cms.Model.Mappings.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class MailController : BaseController
    {
        private MailService _mailService;
        private MailUserService _mailUserService;
        private UserService _userService;
        private UserContactService _userContactService;
        private AttachmentFileService _attachmentFileService;
        private EventLoger _eventLoger;

        public MailController()
        {
            _mailService = new MailService();
            _mailUserService = new MailUserService();
            _userContactService = new UserContactService();
            _attachmentFileService = new AttachmentFileService();
            _userService = new UserService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Inbox(bool? status)
        {
            var onlineContact = _userContactService.GetList(u => u.UserId == SessionData.Current.User.Id).Select(x => x.ContactUserDto).Take(20).ToList();
            ViewBag.onlineContact = onlineContact;

            var model = _mailService.GetInbox(SessionData.Current.User.Id);

            ViewBag.HasRightToLeftUI = SessionData.Current.HasRightToLeftUI;
            ViewBag.UnreadMessageCount = _mailService.GetUnreadInboxCount(SessionData.Current.User.Id);

            if (status != null)
            {
                if (status == true)
                {
                    ViewBag.Status = true;
                    ViewBag.Message = "پیام شما با موفقیت ارسال شد.";
                    ViewBag.Success = "ارسال شد!";
                }
                else if (status == false)
                {
                    ViewBag.Status = false;
                    ViewBag.Message = "خطا در ارسال پیام";
                    ViewBag.Success = "خطا!";
                }
            }
            return View(model);
        }

        public ActionResult Outbox()
        {
            var onlineContact = _userContactService.GetList(u => u.UserId == SessionData.Current.User.Id).Select(x => x.ContactUserDto).Take(20).ToList();
            ViewBag.onlineContact = onlineContact;

            var model = _mailService.GetOutbox(SessionData.Current.User.Id);

            ViewBag.HasRightToLeftUI = SessionData.Current.HasRightToLeftUI;
            ViewBag.UnreadMessageCount = _mailService.GetUnreadInboxCount(SessionData.Current.User.Id);

            return View(model);
        }

        public ActionResult Detail(Guid mailId, bool isReceiver)
        {
            var onlineContact = _userContactService.GetList(u => u.UserId == SessionData.Current.User.Id).Select(x => x.ContactUserId).Take(20).ToList();
            ViewBag.onlineContact = onlineContact;

            var model = _mailService.GetMessageDetail(mailId, isReceiver);

            ViewBag.IsReceiver = (isReceiver ? "true" : "false");
            ViewBag.HasRightToLeftUI = SessionData.Current.HasRightToLeftUI;
            ViewBag.UnreadMessageCount = _mailService.GetUnreadInboxCount(SessionData.Current.User.Id);
            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadAttachment(Guid attachmentGuid)
        {
            var attachment = _attachmentFileService.Get(x => x.Id == attachmentGuid);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = attachment.Name,
                Size = attachment.Size,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(attachment.Binary, "application/" + attachment.Extension.Substring(1));
        }

        public ActionResult Compose(Guid? ReceiverId)
        {
            var onlineContact = _userContactService.GetList(u => u.UserId == SessionData.Current.User.Id).Select(x => x.ContactUserDto).Take(20).ToList();
            ViewBag.onlineContact = onlineContact;

            ViewBag.CurrentUserId = SessionData.Current.User.Id;
            ViewBag.ReceiverId = ReceiverId;

            if (ReceiverId != null)
                ViewBag.ReceiverUserId = _userService.Get(u => u.Id == ReceiverId).Id;
            else
                ViewBag.ReceiverUserId = "";

            // if user is admin
            var users = _userService.GetList(u => !u.IsDeleted && !u.BuiltIn).OrderByDescending(o => o.CreateDate).ToList();
            ViewBag.UserList = users;

            ViewBag.UnreadMessageCount = _mailService.GetUnreadInboxCount(SessionData.Current.User.Id);
            return View();
        }


        [HttpPost]
        public ActionResult DeleteMail(Guid[] MailId, bool IsReceiver)
        {
            try
            {

                if (MailId.Count() > 0)
                {
                    foreach (var item in MailId)
                    {
                        var mail = _mailUserService.Get(q => q.Id == item);

                        if (IsReceiver)
                            mail.DeletedByReceiver = true;
                        else
                        {
                            var mails = _mailUserService.GetList(q => q.MailId == mail.MailId);
                            foreach (var sendMail in mails.MaptoEntities())
                            {
                                sendMail.DeletedBySender = true;
                                _mailUserService.Update(sendMail);
                            }
                        }

                        _mailUserService.Save();

                        ViewBag.Message = "پیام(های) انتخاب شده با موفقیت حذف شد.";
                        ViewBag.Success = "حذف شد!";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "خطا در حذف پیام";
                ViewBag.Success = "خطا!";
                _eventLoger.LogEvent(EventType.Error, SessionData.Current.User.Id, SessionData.Current.User.UserName, "MailController", "DeleteMail", ex.Message.ToString(), HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compose(string mailTo, string mailSubject, string mailText, List<HttpPostedFileBase> files)
        {
            var Status = false;
            try
            {
                using (DataContext dataContext = new DataContext())
                {
                    var attachmentIdList = new List<Guid>();
                    var UserId = mailTo.Split(',');

                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                var attachmentGuid = Guid.NewGuid();
                                byte[] fileData = null;
                                var fileSize = file.ContentLength;
                                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(file.ContentLength);
                                }

                                int imageWidth = 0;
                                int imageHeight = 0;

                                if (file.ContentType.Contains("image"))
                                {
                                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(fileData));
                                    imageWidth = img.Width;
                                    imageHeight = img.Height;
                                }

                                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, attachmentGuid, System.IO.Path.GetFileName(file.FileName),
                                    System.IO.Path.GetExtension(file.FileName), fileSize, MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName)),
                                    imageWidth, imageHeight, "mail AttachmentFile", "", fileData, DateTime.Now);

                                attachmentIdList.Add(attachmentId);
                            }
                        }
                    }

                    if (UserId.Count() > 0)
                    {
                        var receiverList = new List<Guid>();
                        foreach (var item in UserId)
                        {
                            receiverList.Add(_userService.Get(u => u.UserName.ToLower() == item.ToLower()).Id);
                        }

                        var emailProvider = new EmailProvider();
                        emailProvider.SendMail(SessionData.Current.User.UserName, mailSubject, mailText, false);

                        Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Status = false;

                _eventLoger.LogEvent(EventType.Error, SessionData.Current.User.Id, SessionData.Current.User.UserName, "MailController", "Compose", ex.Message.ToString(), HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            }

            return RedirectToAction("Inbox", new { status = Status });
        }
    }
}