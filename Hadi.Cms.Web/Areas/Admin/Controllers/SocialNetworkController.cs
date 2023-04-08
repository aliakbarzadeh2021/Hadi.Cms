using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using Hadi.Cms.Web.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Configuration;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت شبکه اجتماعی
    /// </summary>
    public class SocialNetworkController : Controller
    {
        private readonly SocialNetworkService _socialNetworkService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;
        public SocialNetworkController()
        {
            _socialNetworkService = new SocialNetworkService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست شبکه های اجتماعی
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var socialNetworks = _socialNetworkService.GetList(sn => !sn.IsDeleted, sn => sn.OrderByDescending(o => o.CreatedDate));

            foreach(var socialNetwork in socialNetworks)
                if (socialNetwork.ImageGuid.HasValue)
                        socialNetwork.ImageSource = _attachmentFileService.GetAttachmentSourceValue(socialNetwork.ImageGuid);

            var pagedList = socialNetworks.ToPagedList(pageNumber, pageSize);
            ViewBag.PageNumber = pageNumber;

            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت شبکه اجتماعی جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new SocialNetworkCreateCommand());
        }


        /// <summary>
        /// ثبت شبکه اجتماعی جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="socialNetworkImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SocialNetworkCreateCommand command, HttpPostedFileBase socialNetworkImage)
        {
            if (ModelState.IsValid)
            {
                if (socialNetworkImage != null && socialNetworkImage.ContentLength > 0)
                {
                    byte[] imageData = null;
                    var fileSize = socialNetworkImage.ContentLength;
                    using (var binaryReader = new BinaryReader(socialNetworkImage.InputStream))
                        imageData = binaryReader.ReadBytes(socialNetworkImage.ContentLength);

                    var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(socialNetworkImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                        Path.GetFileName(socialNetworkImage.FileName),
                        Path.GetExtension(socialNetworkImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "SocialNetwork - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.ImageGuid = attachmentId;
                }

                var newSocialNetwork = new SocialNetwork()
                {
                    Title = command.Title,
                    CreatedBy = SessionData.Current.User.Id,
                    IsActive = command.IsActive,
                    IsDeleted = false,
                    Link = command.Link,
                    Source = command.Source?.PersianToEnglishNumber(),
                    ImageGuid = command.ImageGuid
                };

                _socialNetworkService.Insert(newSocialNetwork);
                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "SocialNetworkController", "Create", "Success Create Social Network", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }
            return View(command);
        }


        /// <summary>
        /// صفحه ی ویرایش شبکه ی اجتماعی
        /// </summary>
        /// <param name="socialNetworkId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var socialNetwork = _socialNetworkService.GetByID(id);
            if (socialNetwork == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            var model = new SocialNetworkEditCommand()
            {
                Id = socialNetwork.Id,
                Title = socialNetwork.Title,
                IsActive = socialNetwork.IsActive,
                Link = socialNetwork.Link,
                Source = socialNetwork.Source?.PersianToEnglishNumber(),
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(socialNetwork.ImageGuid),
                ImageGuid = socialNetwork.ImageGuid
            };
            return View(model);
        }

        /// <summary>
        /// ویرایش شبکه اجتماعی
        /// </summary>
        /// <param name="command"></param>
        /// <param name="socialNetworkImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SocialNetworkEditCommand command, HttpPostedFileBase socialNetworkImage)
        {
            if (ModelState.IsValid)
            {
                var socialNetwork = _socialNetworkService.GetByID(command.Id);
                if (socialNetwork == null)
                    return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

                if (socialNetworkImage != null && socialNetworkImage.ContentLength > 0)
                {
                    if (socialNetwork.ImageGuid.HasValue)
                        _attachmentFileService.RemoveAttachment(socialNetwork.ImageGuid.Value);

                    byte[] imageData = null;
                    var fileSize = socialNetworkImage.ContentLength;
                    
                    using (var binaryReader = new BinaryReader(socialNetworkImage.InputStream))
                        imageData = binaryReader.ReadBytes(socialNetworkImage.ContentLength);

                    var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(socialNetworkImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                        Path.GetFileName(socialNetworkImage.FileName),
                        Path.GetExtension(socialNetworkImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "SocialNetwork - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.ImageGuid = attachmentId;
                }

                _socialNetworkService.Update(command, socialNetwork, SessionData.Current.User.Id);
                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "SocialNetworkController", "Edit", "Success Update Social Network", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }
            return View(command);
        }


        public ActionResult ChangeStatus(Guid id)
        {
            var socialNetwork = _socialNetworkService.GetByID(id).MapToEntity();
            if (socialNetwork == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            socialNetwork.IsActive = !socialNetwork.IsActive;
            _socialNetworkService.Update(socialNetwork);
            _socialNetworkService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف شبکه اجتماعی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var socialNetwork = _socialNetworkService.GetByID(id).MapToEntity();

                _socialNetworkService.Delete(socialNetwork.Id);
                return Json(new
                {
                    Message = "شبکه اجتماعی انتخاب شده با موفقیت حذف شد",
                    Success = Strings.Success,
                    Type = "success"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error,
                    Type = "error"

                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}