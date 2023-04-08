using Hadi.Cms.ApplicationService.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت ایتم های اسلایدر
    /// </summary>
    public class SliderItemsController : Controller
    {
        private readonly SliderItemService _sliderItemService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public SliderItemsController()
        {
            _sliderItemService = new SliderItemService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست ایتم های اسلایدر
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var sliderItems = _sliderItemService.GetList(s =>
                    !s.IsDeleted && s.SliderId == id && s.CreatedBy == SessionData.Current.User.Id,
                s => s.OrderBy(o => o.OrderId));

            foreach (var item in sliderItems)
                item.AttachmentImageSource = item.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(item.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = sliderItems.ToPagedList(pageNumber, pageSize);

            ViewBag.SliderId = id;

            ViewBag.PageNumber = pageNumber;

            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");
            TempData["Id"] = id;
            TempData.Keep("Id");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ویرایش ایتم های اسلایدر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            return View(new SliderItemCreateCommand
            {
                SliderId = id
            });
        }

        /// <summary>
        /// ثبت ایتم برای اسلایدر
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sliderItemImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SliderItemCreateCommand command, HttpPostedFileBase sliderItemImage)
        {
            #region Insert sldier item image

            if (sliderItemImage != null && sliderItemImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = sliderItemImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sliderItemImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sliderItemImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sliderItemImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sliderItemImage.FileName),
                    System.IO.Path.GetExtension(sliderItemImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SliderItemImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            #endregion

            _sliderItemService.CreateNewSliderItem(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "SliderItemsController", "Create", "Success Create SliderItems", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش ایتم اسلایدر
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id, Guid sliderId)
        {
            var sliderItem = _sliderItemService.Get(id);
            if (sliderItem == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new SliderItemEditCommand
            {
                Id = sliderItem.Id,
                SliderId = sliderId,
                OrderId = sliderItem.OrderId,
                Title = sliderItem.Title?.Replace("<br/>", "\r\n"),
                SubTitle = sliderItem.SubTitle,
                ButtonText = sliderItem.ButtonText,
                ButtonLink = sliderItem.ButtonLink,
                ButtonCss = sliderItem.ButtonCss,
                Description = sliderItem.Description?.Replace("<br/>", "\r\n"),
                AttachmentImageId = sliderItem.AttachmentImageId,
                AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(sliderItem.AttachmentImageId),
                IsActive = sliderItem.IsActive,
            });
        }

        /// <summary>
        /// ویرایش ایتم اسلایدر
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sliderItemImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SliderItemEditCommand command, HttpPostedFileBase sliderItemImage)
        {
            var sliderItem = _sliderItemService.Get(command.Id).MapToEntity();

            #region Update slider item image

            if (sliderItemImage != null && sliderItemImage.ContentLength > 0)
            {
                if (sliderItem.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(sliderItem.AttachmentImageId.Value);

                byte[] imageData;
                var fileSize = sliderItemImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(sliderItemImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(sliderItemImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sliderItemImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(sliderItemImage.FileName),
                    System.IO.Path.GetExtension(sliderItemImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "SliderItemImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            #endregion

            _sliderItemService.UpdateSliderItem(sliderItem, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "SliderItemsController", "Edit", "Success Edit SliderItems", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id, Guid sliderId)
        {
            var sliderItem = _sliderItemService.Get(id).MapToEntity();
            if (sliderItem == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            sliderItem.IsActive = !sliderItem.IsActive;
            _sliderItemService.Update(sliderItem);
            _sliderItemService.Save();
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف ایتم اسلایدر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var sliderItem = _sliderItemService.Get(id);

                #region Remove slider item image

                if (sliderItem.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(sliderItem.AttachmentImageId.Value);

                #endregion

                _sliderItemService.Delete(sliderItem.Id);
                _sliderItemService.Save();

                return Json(new
                {
                    Message = Strings.SliderItemModel_Delete_Successfully,
                    Success = Strings.Success,
                    Type = "success"
                });
            }

            catch
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