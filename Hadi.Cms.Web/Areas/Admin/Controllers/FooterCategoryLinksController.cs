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
    /// مدیریت لینک های دسته بندی
    /// </summary>
    public class FooterCategoryLinksController : Controller
    {
        private readonly FooterCategoryLinkService _footerCategoryLinkService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public FooterCategoryLinksController()
        {
            _footerCategoryLinkService = new FooterCategoryLinkService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست لینک ها
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var footerCategoryLinks = _footerCategoryLinkService.GetList(
                f => f.FooterCategoryId == id && f.CreatedBy == SessionData.Current.User.Id && !f.IsDeleted,
                o => o.OrderByDescending(f => f.CreatedDate));

            foreach (var item in footerCategoryLinks)
                item.ImageAttachmentSource = item.ImageAttachmentGuid.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(item.ImageAttachmentGuid) : "/PhysicalStorage/no_image.png";


            var pagedList = footerCategoryLinks.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.FooterCategoryId = id;
            
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");
            
            TempData["Id"] = id;
            TempData.Keep("Id");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت لینک
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            return View(new FooterCategoryLinkCreateCommand
            {
                FooterCategoryId = id
            });
        }

        /// <summary>
        /// ثبت لینک
        /// </summary>
        /// <param name="command"></param>
        /// <param name="linkImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FooterCategoryLinkCreateCommand command, HttpPostedFileBase linkImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            #region Insert link image in attachment file table

            if (linkImage != null && linkImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = linkImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(linkImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(linkImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(linkImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(linkImage.FileName),
                    System.IO.Path.GetExtension(linkImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "LinkImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageAttachmentGuid = attachmentId;
            }

            #endregion

            _footerCategoryLinkService.AddFooterCategoryLink(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FooterCategoryLinksController", "Create", "Success Create Footer Category Link", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()) , pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش لینک
        /// </summary>
        /// <param name="id"></param>
        /// <param name="footerCategoryId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id, Guid footerCategoryId)
        {
            var footerCategoryLink = _footerCategoryLinkService.Get(id);
            if (footerCategoryLink == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new FooterCategoryLinkEditCommand()
            {
                Id = footerCategoryLink.Id,
                FooterCategoryId = footerCategoryLink.FooterCategoryId,
                Link = footerCategoryLink.Link,
                LinkText = footerCategoryLink.LinkText,
                ImageAttachmentGuid = footerCategoryLink.ImageAttachmentGuid,
                ImageAttachmentSource = _attachmentFileService.GetAttachmentSourceValue(footerCategoryLink.ImageAttachmentGuid),
                IsActive = footerCategoryLink.IsActive
            });
        }

        /// <summary>
        /// ویرایش لینک
        /// </summary>
        /// <param name="command"></param>
        /// <param name="linkImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FooterCategoryLinkEditCommand command, HttpPostedFileBase linkImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            var footerCategoryLink = _footerCategoryLinkService.Get(command.Id).MapToEntity();

            #region Insert link image in attachment file table

            if (linkImage != null && linkImage.ContentLength > 0)
            {
                if (footerCategoryLink.ImageAttachmentGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(footerCategoryLink.ImageAttachmentGuid.Value);

                byte[] imageData;
                var fileSize = linkImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(linkImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(linkImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(linkImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(linkImage.FileName),
                    System.IO.Path.GetExtension(linkImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "LinkImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageAttachmentGuid = attachmentId;
            }

            #endregion

            _footerCategoryLinkService.UpdateFooterCategoryLink(footerCategoryLink, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FooterCategoryLinksController", "Edit", "Success Edit Footer Category Link", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن لینک
        /// </summary>
        /// <param name="id"></param>
        /// <param name="footerCategoryId"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id, Guid footerCategoryId)
        {
            var footerCategoryLink = _footerCategoryLinkService.Get(id).MapToEntity();
            if (footerCategoryLink == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            footerCategoryLink.IsActive = !footerCategoryLink.IsActive;
            _footerCategoryLinkService.Update(footerCategoryLink);
            _footerCategoryLinkService.Save();

            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف لینک
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var footerCategoryLink = _footerCategoryLinkService.Get(id);

                #region Remove link image

                if (footerCategoryLink.ImageAttachmentGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(footerCategoryLink.ImageAttachmentGuid.Value);

                #endregion

                _footerCategoryLinkService.Delete(footerCategoryLink.Id);
                _footerCategoryLinkService.Save();

                return Json(new
                {
                    Message = Strings.FooterLinkModel_Delete_Successfully,
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