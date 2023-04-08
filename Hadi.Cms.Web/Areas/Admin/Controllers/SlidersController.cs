using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت اسلایدرها
    /// </summary>
    public class SlidersController : Controller
    {
        private readonly SliderService _sliderService;
        private readonly SliderItemService _sliderItemService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public SlidersController()
        {
            _sliderService = new SliderService();
            _sliderItemService = new SliderItemService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست اسلایدرها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var sliders = _sliderService.GetList(s => !s.IsDeleted && s.CreatedBy == SessionData.Current.User.Id,
                o => o.OrderByDescending(s => s.CreatedDate));
            var pagedList = sliders.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت اسلایدر چدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new SliderCreateCommand());
        }

        /// <summary>
        /// ثبت اسلایدر جدید
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SliderCreateCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var existsTitle = _sliderService.Any(s => !s.IsDeleted && s.Title == command.Title);
            if (existsTitle)
            {
                ModelState.AddModelError("Title", Strings.SliderModel_Title_Exists);
                return View(command);
            }

            _sliderService.CreateNewSlider(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "DepartmentsController", "Create", "Success Create Slider", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش اسلایدر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var slider = _sliderService.Get(id);
            if (slider == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new SliderEditCommand
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description?.Replace("<br/>", "\r\n"),
                IsActive = slider.IsActive
            });
        }

        /// <summary>
        /// ویرایش اسلایدر
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SliderEditCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var existsTitle = _sliderService.Any(s => s.Id != command.Id && !s.IsDeleted && s.Title == command.Title);
            if (existsTitle)
            {
                ModelState.AddModelError("Title", Strings.SliderModel_Title_Exists);
                return View(command);
            }

            var slider = _sliderService.Get(command.Id).MapToEntity();

            _sliderService.UpdateSlider(slider, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "DepartmentsController", "Edit", "Success Edit Slider", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var slider = _sliderService.Get(id).MapToEntity();
            if (slider == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            slider.IsActive = !slider.IsActive;
            _sliderService.Update(slider);
            _sliderService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف اسلایدر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var slider = _sliderService.Get(id);

                #region Remove slider items

                var sliderItems = _sliderItemService.GetList(s => s.SliderId == slider.Id);
                foreach (var sliderItem in sliderItems)
                {
                    if (sliderItem.AttachmentImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(sliderItem.AttachmentImageId.Value);

                    _sliderItemService.Delete(sliderItem.Id);
                }

                _sliderItemService.Save();

                #endregion

                _sliderService.Delete(slider.Id);
                _sliderService.Save();

                return Json(new
                {
                    Message = Strings.SliderModel_Delete_Successfully,
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