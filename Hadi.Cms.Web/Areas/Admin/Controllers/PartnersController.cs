using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت همکاران
    /// </summary>
    public class PartnersController : Controller
    {
        private readonly PartnerService _partnerService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;

        public PartnersController()
        {
            _partnerService = new PartnerService();
            _eventLoger = new EventLoger();
            _attachmentFileService = new AttachmentFileService();
        }

        /// <summary>
        /// دریافت لیست همکاران
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var partners = _partnerService.GetList(p => !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id,
                o => o.OrderByDescending(p => p.CreatedDate));

            foreach (var item in partners)
                item.AttachmentImageSource = item.AttachmentImageGuid.HasValue ? _attachmentFileService.GetAttachmentSourceValue(item.AttachmentImageGuid) : "/PhysicalStorage/no_image.png";

            var pagedList = partners.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت همکار
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new PartnerCreateCommand());
        }

        /// <summary>
        /// ثبت همکار
        /// </summary>
        /// <param name="command"></param>
        /// <param name="partnerLogo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PartnerCreateCommand command, HttpPostedFileBase partnerLogo)
        {
            if (!ModelState.IsValid)
                return View(command);


            #region Insert partner logo in attachmeng file table

            if (partnerLogo != null && partnerLogo.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = partnerLogo.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(partnerLogo.InputStream))
                {
                    imageData = binaryReader.ReadBytes(partnerLogo.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(partnerLogo.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(partnerLogo.FileName),
                    System.IO.Path.GetExtension(partnerLogo.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "partnerImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageGuid = attachmentId;
            }

            #endregion


            _partnerService.AddNewPartner(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "PartnersController", "Create", "Success Create Partner", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index" , new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1});
        }


        /// <summary>
        /// صفحه ی ویرایش همکار
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var partner = _partnerService.Get(id);
            if (partner == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new PartnerEditCommand
            {
                Id = partner.Id,
                Name = partner.Name,
                ToolTip = partner.ToolTip,
                Link = partner.Link,
                IsActive = partner.IsActive,
                AttachmentImageGuid = partner.AttachmentImageGuid,
                AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(partner.AttachmentImageGuid)
            });
        }

        /// <summary>
        /// ویرایش همکار
        /// </summary>
        /// <param name="command"></param>
        /// <param name="partnerLogo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartnerEditCommand command, HttpPostedFileBase partnerLogo)
        {
            if (!ModelState.IsValid)
                return View(command);

            var partner = _partnerService.Get(command.Id).MapToEntity();

            #region Insert partner logo in attachmeng file table

            if (partnerLogo != null && partnerLogo.ContentLength > 0)
            {
                if (partner.AttachmentImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(partner.AttachmentImageGuid.Value);

                byte[] imageData;
                var fileSize = partnerLogo.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(partnerLogo.InputStream))
                {
                    imageData = binaryReader.ReadBytes(partnerLogo.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(partnerLogo.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(partnerLogo.FileName),
                    System.IO.Path.GetExtension(partnerLogo.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "partnerImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageGuid = attachmentId;
            }

            #endregion

            _partnerService.UpdatePartner(partner, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "PartnersController", "Edit", "Success Edit Partner", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var partner = _partnerService.Get(id).MapToEntity();
            if (partner == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            partner.IsActive = !partner.IsActive;
            _partnerService.Update(partner);
            _partnerService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف همکار
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var partner = _partnerService.Get(id);

                #region Remove partner logo

                if (partner.AttachmentImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(partner.AttachmentImageGuid.Value);

                #endregion

                _partnerService.Delete(partner.Id);
                _partnerService.Save();

                return Json(new
                {
                    Message = Strings.ParterModel_Delete_Successfully,
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