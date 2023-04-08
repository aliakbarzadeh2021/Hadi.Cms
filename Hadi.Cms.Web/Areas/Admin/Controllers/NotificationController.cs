using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Language.Resources;


namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class NotificationController : BaseController
    {
        private NotificationService _notificationService;
        private UserService _userService;
        private EventLoger _eventLoger;

        public NotificationController()
        {
            _notificationService = new NotificationService();
            _userService = new UserService();
            _eventLoger = new EventLoger();
        }


        public ActionResult List(int pageNumber = 1)
        {
            var pageSize = 5;
            var model = new List<NotificationModel>();
            var notification = _notificationService.GetList(n => !n.IsDelete);

            foreach (var item in notification)
            {
                model.Add(new NotificationModel()
                {
                    Id = item.Id,
                    Text = item.Text,
                    Sender = _userService.Get(u => u.Id == item.SenderUserId).MaptoEntity(),
                    Receiver = _userService.Get(u => u.Id == item.ReceiverUserId).MaptoEntity(),
                    SendTime = item.SendDateTime,
                    ReceiveTime = item.ReadDateTime,
                    IsPublic = item.Is,
                    Status = item.Unread,
                    NewsId = item.NewsId
                });
            }

            var pagedList = model.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        public ActionResult Create()
        {
            ViewBag.Receivers = _userService.GetList(u => !u.IsDeleted && u.UserName != "Administrator" && u.UserName != "publicuser");
            return View(new NotificationCommand());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NotificationCommand model)
        {
            if (ModelState.IsValid)
            {
                model.Sender = SessionData.Current.User;
                model.SendTime = DateTime.Now;


                var result = _notificationService.CreateNewNotification(model);

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "NotificationController", "Create", "Success Create Notification", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                if (result)
                {
                    ViewBag.Message = Strings.Notification_Created_Successfully;
                    ViewBag.Success = "1";
                }
                else
                {
                    ViewBag.Message = Strings.Notification_Created_Error;
                    ViewBag.Success = "0";
                }
            }
            ViewBag.Receivers = _userService.GetList(u => !u.IsDeleted && u.UserName != "Administrator" && u.UserName != "publicuser");
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteNotification(Guid NotificationId)
        {
            try
            {

                var item = _notificationService.Get(n => n.Id == NotificationId);

                if (item != null)
                {
                    _notificationService.DeleteById(item.Id);
                    _notificationService.Save();

                    ViewBag.Message = Strings.Notification_Delete_Successfully;
                    ViewBag.Success = Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.Notification_Dont_Have_Permission;
                    ViewBag.Success = Strings.Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.Notification_Delete_Error;
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