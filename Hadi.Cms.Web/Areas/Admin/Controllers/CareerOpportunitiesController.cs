using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Log.EventLoger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Enums;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت فرصت های شغلی
    /// </summary>
    public class CareerOpportunitiesController : Controller
    {
        private readonly CareerOpportunityService _careerOpportunityService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;
        private readonly ResumeService _resumeService;

        public CareerOpportunitiesController()
        {
            _careerOpportunityService = new CareerOpportunityService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
            _resumeService = new ResumeService();
        }

        /// <summary>
        /// دریافت لیست فرصت های شغلی
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var careerOpportunities = _careerOpportunityService.GetList(
                c => !c.IsDeleted && c.CreatedBy == SessionData.Current.User.Id,
                o => o.OrderByDescending(c => c.CreatedDate), c => c.Resumes);

            foreach (var careerOpportunity in careerOpportunities)
            {
                careerOpportunity.CareerOpportunityImageSource = careerOpportunity.CareerOpportunityImageGuid.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(careerOpportunity.CareerOpportunityImageGuid) : "/PhysicalStorage/no_image.png";

                careerOpportunity.CareerOpportunityFaDisplayName = EnumHelper.GetDisplayForEnumValue(careerOpportunity.CareerOpportunityType);
            }

            var pagedList = careerOpportunities.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت فرصت شغلی
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var careerOpportunitiesType =
                Enum.GetValues(typeof(CareerOpportunityType)).OfType<CareerOpportunityType>().ToList();

            var careerOpportunitiesList = new List<CareerOpportunityTypeDto>();

            foreach (var careerOpportunityType in careerOpportunitiesType)
            {
                var enumDisplayAttr = EnumHelper.GetDisplayForEnumValue(careerOpportunityType);
                careerOpportunitiesList.Add(new CareerOpportunityTypeDto()
                {
                    Value = (int)careerOpportunityType,
                    Name = enumDisplayAttr
                });
            }

            ViewBag.CareerOpportunities = careerOpportunitiesList;

            return View(new CareerOpportunityCreateCommand());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="careerOpportunityImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CareerOpportunityCreateCommand command, HttpPostedFileBase careerOpportunityImage)
        {
            if (!ModelState.IsValid)
            {
                var careerOpportunitiesType =
                    Enum.GetValues(typeof(CareerOpportunityType)).OfType<CareerOpportunityType>().ToList();

                var careerOpportunitiesList = new List<CareerOpportunityTypeDto>();

                foreach (var careerOpportunityType in careerOpportunitiesType)
                {
                    var enumDisplayAttr = EnumHelper.GetDisplayForEnumValue(careerOpportunityType);
                    careerOpportunitiesList.Add(new CareerOpportunityTypeDto()
                    {
                        Value = (int)careerOpportunityType,
                        Name = enumDisplayAttr
                    });
                }

                ViewBag.CareerOpportunities = careerOpportunitiesList;

                return View(command);
            }

            #region Insert career opportunity image

            if (careerOpportunityImage != null && careerOpportunityImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = careerOpportunityImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(careerOpportunityImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(careerOpportunityImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(careerOpportunityImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(careerOpportunityImage.FileName),
                    System.IO.Path.GetExtension(careerOpportunityImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "CareerOpportunityImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.CareerOpportunityImageGuid = attachmentId;
            }

            #endregion

            _careerOpportunityService.CreateNewCareerOpportunity(command, SessionData.Current.User.Id);

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName,
                "CareerOpportunitiesController", "Create", "Success Create CareerOpportunity",
                HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش فرصت شغلی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var careerOpportunity = _careerOpportunityService.Get(id);
            if (careerOpportunity == null)
                return RedirectToAction("Index");


            var careerOpportunitiesType =
                Enum.GetValues(typeof(CareerOpportunityType)).OfType<CareerOpportunityType>().ToList();

            var careerOpportunitiesList = new List<CareerOpportunityTypeDto>();

            foreach (var careerOpportunityType in careerOpportunitiesType)
            {
                var enumDisplayAttr = EnumHelper.GetDisplayForEnumValue(careerOpportunityType);
                careerOpportunitiesList.Add(new CareerOpportunityTypeDto()
                {
                    Value = (int)careerOpportunityType,
                    Name = enumDisplayAttr
                });
            }

            ViewBag.CareerOpportunities = careerOpportunitiesList;

            return View(new CareerOpportunityEditCommand()
            {
                Id = careerOpportunity.Id,
                Title = careerOpportunity.Title,
                Description = careerOpportunity.Description,
                CareerOpportunityImageGuid = careerOpportunity.CareerOpportunityImageGuid,
                CareerOpportunityImageSource = _attachmentFileService.GetAttachmentSourceValue(careerOpportunity.CareerOpportunityImageGuid),
                CareerOpportunityType = careerOpportunity.CareerOpportunityType,
                IsActive = careerOpportunity.IsActive
            });
        }

        /// <summary>
        /// ویرایش فرصت شغلی
        /// </summary>
        /// <param name="command"></param>
        /// <param name="careerOpportunityImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CareerOpportunityEditCommand command, HttpPostedFileBase careerOpportunityImage)
        {
            if (!ModelState.IsValid)
            {
                var careerOpportunitiesType =
                    Enum.GetValues(typeof(CareerOpportunityType)).OfType<CareerOpportunityType>().ToList();

                var careerOpportunitiesList = new List<CareerOpportunityTypeDto>();

                foreach (var careerOpportunityType in careerOpportunitiesType)
                {
                    var enumDisplayAttr = EnumHelper.GetDisplayForEnumValue(careerOpportunityType);
                    careerOpportunitiesList.Add(new CareerOpportunityTypeDto()
                    {
                        Value = (int)careerOpportunityType,
                        Name = enumDisplayAttr
                    });
                }

                return View(command);
            }

            var careerOpportunity = _careerOpportunityService.Get(command.Id).MapToEntity();
            if (careerOpportunity == null)
                return HttpNotFound("Career opportunity not found . ");

            #region Update career opportunityimage

            if (careerOpportunityImage != null && careerOpportunityImage.ContentLength > 0)
            {
                if (command.CareerOpportunityImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(command.CareerOpportunityImageGuid.Value);

                byte[] imageData;
                var fileSize = careerOpportunityImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(careerOpportunityImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(careerOpportunityImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(careerOpportunityImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(careerOpportunityImage.FileName),
                    System.IO.Path.GetExtension(careerOpportunityImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "CareerOpportunityImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.CareerOpportunityImageGuid = attachmentId;

            }

            #endregion

            _careerOpportunityService.UpdateCareerOpportunity(careerOpportunity, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName,
                "CareerOpportunitiesController", "Edit", "Success Edit CareerOpportunity",
                HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var careerOpportunity = _careerOpportunityService.Get(id).MapToEntity();
            if (careerOpportunity == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });


            careerOpportunity.IsActive = !careerOpportunity.IsActive;
            _careerOpportunityService.Update(careerOpportunity);
            _careerOpportunityService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

        }

        /// <summary>
        /// حذف فرصت شغلی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var careerOpportunity = _careerOpportunityService.Get(id).MapToEntity();

                #region Remove resumes from career opportunity

                var resumes = _resumeService.GetList(r => r.CareerOpportunityId == careerOpportunity.Id);
                foreach (var resume in resumes)
                {
                    _resumeService.Delete(resume.Id);
                }
                _resumeService.Save();

                #endregion

                _careerOpportunityService.Delete(careerOpportunity.Id);
                _careerOpportunityService.Save();

                return Json(new
                {
                    Message = Strings.CareerOpportunityModel_Delete_Successfully,
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