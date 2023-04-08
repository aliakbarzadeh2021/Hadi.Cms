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
    public class ServiceReceiversController : Controller
    {
        private readonly ServiceReceiverService _serviceReceiverService;
        private readonly ServiceReceiverServiceService _serviceReceiverServiceService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;
        private readonly ServiceService _serviceService;

        public ServiceReceiversController()
        {
            _serviceReceiverServiceService = new ServiceReceiverServiceService();
            _serviceReceiverService = new ServiceReceiverService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
            _serviceService = new ServiceService();
        }

        /// <summary>
        /// دریافت لیست سرویس گیر
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var serviceReceivers = _serviceReceiverService.GetList(s => !s.IsDeleted);

            foreach (var serviceReceiver in serviceReceivers)
                serviceReceiver.AttachmentImageSource = serviceReceiver.AttachmentImageId.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(serviceReceiver.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = serviceReceivers.OrderByDescending(s => s.CreatedDate).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت سرویس گیرنده
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Services = _serviceService.GetList(s =>
                s.IsActive && !s.IsDeleted && s.CreatedBy == SessionData.Current.User.Id);
            return View(new ServiceReceiverCreateCommand());
        }

        /// <summary>
        /// ثبت سرویس گیرنده
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceReceiverCreateCommand command, HttpPostedFileBase file)
        {
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
                    imageWidth, imageHeight, "ServiceReceiversImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            #endregion
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ServiceReceiversController", "Create", "Success Create Service Receiver", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            var newServiceReceiverId = _serviceReceiverService.CreateNewServiceReceiver(command, SessionData.Current.User.Id);

            command.ServicesId?.RemoveAll(s => s == Guid.Empty);
            _serviceReceiverServiceService.AssignServiceToServiceReceiver(newServiceReceiverId, command.ServicesId,
                SessionData.Current.User.Id);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش سرویس گیرنده
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var serviceReceiver = _serviceReceiverService.GetList(s => s.IsActive && !s.IsDeleted && s.Id == id, null,
                    s => s.ServiceReceiverServices, s => s.ServiceReceiverServices.Select(sr => sr.Service))
                .FirstOrDefault();

            if (serviceReceiver == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Fill services

            List<string> servicesList = new List<string>();
            ViewBag.Services = _serviceService.GetList(s =>
                !s.IsDeleted && s.IsActive && s.CreatedBy == SessionData.Current.User.Id);
            foreach (var item in serviceReceiver.ServiceReceiverServiceDto)
            {
                servicesList.Add(item.ServiceId.ToString());
            }
            ViewBag.SelectedService = string.Join(",", servicesList);
            #endregion

            return View(new ServiceReceiverEditCommand()

            {
                Id = serviceReceiver.Id,
                ReceiverName = serviceReceiver.ReceiverName,
                AttachmentImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(serviceReceiver.AttachmentImageId),
                AttachmentImageId = serviceReceiver.AttachmentImageId
            });
        }

        /// <summary>
        /// ویرایش سرویس گیرنده
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceReceiverEditCommand command, HttpPostedFileBase file)
        {
            var serviceReceiver = _serviceReceiverService.Get(command.Id).MapToEntity();
            if (serviceReceiver == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Insert image in attachment file .

            if (file != null && file.ContentLength > 0)
            {
                if (serviceReceiver.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(serviceReceiver.Id);

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
                    imageWidth, imageHeight, "ServiceReceiversImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            #endregion

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ServiceReceiversController", "Edit", "Success Edit Service Receiver", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            _serviceReceiverService.UpdateServiceReceiver(serviceReceiver, command, SessionData.Current.User.Id);

            command.ServicesId?.RemoveAll(s => s == Guid.Empty);
            _serviceReceiverServiceService.AssignServiceToServiceReceiver(serviceReceiver.Id, command.ServicesId,
                SessionData.Current.User.Id);

            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف سرویس گیرنده
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var serviceReceiver = _serviceReceiverService.Get(id);

                #region Remove dependencies

                var serviceReceiverServices =
                    _serviceReceiverServiceService.GetList(s => s.ServiceReceiverId == serviceReceiver.Id);
                foreach (var serviceReceiverService in serviceReceiverServices)
                {
                    _serviceReceiverServiceService.Delete(serviceReceiverService.Id);
                }
                _serviceReceiverServiceService.Save();

                #endregion

                _serviceReceiverService.Delete(serviceReceiver.Id);
                _serviceReceiverService.Save();
                
                return Json(new
                {
                    Message = Strings.ServiceReceiverModel_Delete_Successfully,
                    Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
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