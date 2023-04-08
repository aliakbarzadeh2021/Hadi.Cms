using System;
using System.Linq;
using System.Web;
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
    /// مدیریت کارفرمایان
    /// </summary>
    public class EmployersController : Controller
    {
        private readonly EmployerService _employerService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public EmployersController()
        {
            _employerService = new EmployerService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }
        /// <summary>
        /// لیست کارفرمایان
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var employers = _employerService.GetList(e => !e.IsDeleted && e.CreatedBy == SessionData.Current.User.Id, o => o.OrderByDescending(e => e.CreatedDate));
            var employersPagedList = employers.ToPagedList(pageNumber, pageSize);

            foreach (var employer in employersPagedList)
                employer.LogoSource = employer.LogoGuid != Guid.Empty ? _attachmentFileService.GetAttachmentSourceValue(employer.LogoGuid) : "/PhysicalStorage/no_image.png";

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");
            return View(employersPagedList);
        }

        /// <summary>
        ///صفحه ی ثبت کارفرمای جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new EmployerCreateCommand());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployerCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var employerAlreadyExist = _employerService.Any(e => e.Name == command.Name && !e.IsDeleted);
            if (employerAlreadyExist)
            {
                ModelState.AddModelError("Name", Strings.EmployerModel_ExistsName);
                return View(command);
            }

            if (file != null && file.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    imageData = binaryReader.ReadBytes(file.ContentLength);

                var mimeType = Infrastructure.Helpers.MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                // ثبت لوگو
                var attachmentGuid = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                     System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                     imageWidth, imageHeight, "EmployerLogo - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.LogoGuid = attachmentGuid;
            }

            _employerService.CreateNewEmployer(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "EmployersController", "Create", "Success Create Employer", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش کارفرما
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var employer = _employerService.Get(id);
            if (employer == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });


            var command = new EmployerEditCommand
            {
                Id = employer.Id,
                Name = employer.Name,
                CEOName = employer.CEOName,
                Phone = employer.Phone,
                Address = employer.Address,
                CreatedDate = employer.CreatedDate,
                LogoGuid = employer.LogoGuid,
                LogoSource = _attachmentFileService.GetAttachmentSourceValue(employer.LogoGuid),
                IsActive = employer.IsActive,
            };
            return View(command);
        }

        /// <summary>
        /// ویرایش اطاعات کارفرما
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployerEditCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var employerAlreadyExist = _employerService.Any(e => e.Id != command.Id && e.Name == command.Name);
            if (employerAlreadyExist)
            {
                ModelState.AddModelError("Name", Strings.EmployerModel_ExistsName);
                return View(command);
            }

            var employer = _employerService.Get(command.Id).MapToEntity();

            if (file != null && file.ContentLength > 0)
            {
                _attachmentFileService.RemoveAttachment(command.LogoGuid);

                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }

                var mimeType =
                    Infrastructure.Helpers.MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentGuid = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                     System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                     imageWidth, imageHeight, "EmployerLogo - " + command.Id, "", imageData, DateTime.Now);

                command.LogoGuid = attachmentGuid;
            }
            _employerService.UpdateEmployer(employer, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "EmployersController", "Edit", "Success Update Employer", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// تغییر وضعیت فال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var employer = _employerService.Get(e => e.Id == id).MapToEntity();
            if (employer == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            employer.IsActive = !employer.IsActive;
            _employerService.Update(employer);
            _employerService.Save();
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// حذف کارفرما
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var employer = _employerService.GetList(e => e.Id == id, null).First().MapToEntity();

                employer.IsActive = false;
                employer.IsDeleted = true;
                _employerService.Update(employer);
                _employerService.Save();

                return Json(new
                {
                    Message = Strings.Employer_Delete_Successfully,
                    Success = Strings.Global_Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Global_Error,
                    Type = "error"
                });
            }
        }
    }
}