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
    /// <summary>
    /// مدیریت دپارتمان ها
    /// </summary>
    public class DepartmentsController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ServiceService _serviceService;
        private readonly DepartmentServiceService _departmentServiceService;
        private readonly EmployeeService _employeeService;
        private readonly DepartmentSelectionReasonService _departmentSelectionReasonService;
        private readonly MethodologyService _methodologyService;
        public DepartmentsController()
        {
            _departmentService = new DepartmentService();
            _eventLoger = new EventLoger();
            _attachmentFileService = new AttachmentFileService();
            _serviceService = new ServiceService();
            _departmentServiceService = new DepartmentServiceService();
            _employeeService = new EmployeeService();
            _departmentSelectionReasonService = new DepartmentSelectionReasonService();
            _methodologyService = new MethodologyService();
        }

        /// <summary>
        /// دریافت لیست دپارتمان ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var departments = _departmentService.GetList(d => !d.IsDeleted && d.CreatedBy == SessionData.Current.User.Id);

            foreach (var department in departments)
                department.Source = department.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(department.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = departments.OrderByDescending(d => d.CreatedDate).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// ثبت دپارتمان جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Services = _serviceService.GetList(s =>
                !s.IsDeleted && s.IsActive && s.CreatedBy == SessionData.Current.User.Id);

            return View(new DepartmentCreateCommand());
        }

        /// <summary>
        /// ثبت دپارتمان جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <param name="headerBackgroundImage"></param>
        /// <param name="structureImage"></param>
        /// <param name="joinOurTeamImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentCreateCommand command, HttpPostedFileBase file, HttpPostedFileBase headerBackgroundImage, HttpPostedFileBase structureImage, HttpPostedFileBase joinOurTeamImage)
        {
            if (ModelState.IsValid)
            {
                var existsDepartment = _departmentService.Any(d => d.Title == command.Title);
                if (existsDepartment)
                {
                    ViewBag.Services = _serviceService.GetList(s =>
                        !s.IsDeleted && s.IsActive && s.CreatedBy == SessionData.Current.User.Id);

                    ModelState.AddModelError("Title", Strings.Exists_Title_Department);
                    return View(command);
                }

                #region Insert image in attachment file
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
                        imageWidth, imageHeight, "DepartmentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.AttachmentImageId = attachmentId;
                }

                if (headerBackgroundImage != null && headerBackgroundImage.ContentLength > 0)
                {
                    byte[] imageData;
                    var fileSize = headerBackgroundImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(headerBackgroundImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(headerBackgroundImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(headerBackgroundImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(headerBackgroundImage.FileName),
                        System.IO.Path.GetExtension(headerBackgroundImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "DepartmentHeaderBackgroundImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.DepartmentBackgroundHeaderAttachmentImageId = attachmentId;
                }

                if (structureImage != null && structureImage.ContentLength > 0)
                {
                    byte[] imageData;
                    var fileSize = structureImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(structureImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(structureImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(structureImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(structureImage.FileName),
                        System.IO.Path.GetExtension(structureImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "StructureImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.StructureAttachmentImageId = attachmentId;
                }

                if (joinOurTeamImage != null && joinOurTeamImage.ContentLength > 0)
                {
                    byte[] imageData;
                    var fileSize = joinOurTeamImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(joinOurTeamImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(joinOurTeamImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(joinOurTeamImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(joinOurTeamImage.FileName),
                        System.IO.Path.GetExtension(joinOurTeamImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "JoinOurTeamImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.JoinOurTeamImageAttachmentId = attachmentId;
                }
                #endregion

                var newDepartmentId = _departmentService.CreateDepartment(command, SessionData.Current.User.Id);

                #region Assign service to deparment
                command.ServicesId?.RemoveAll(s => s == Guid.Empty);
                _departmentServiceService.AssignServiceToDepartment(newDepartmentId, command.ServicesId, SessionData.Current.User.Id);
                #endregion

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "DepartmentsController", "Create", "Success Create Department", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            ViewBag.Services = _serviceService.GetList(s =>
                !s.IsDeleted && s.IsActive && s.CreatedBy == SessionData.Current.User.Id);
            return View(command);

        }

        /// <summary>
        /// صفحه ی ویرایش دپارتمان
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var department = _departmentService.GetList(d => d.Id == id, null, d => d.DepartmentServices).FirstOrDefault();
            if (department == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Fill services

            List<string> servicesList = new List<string>();
            foreach (var item in department.DepartmentServiceDto)
            {
                servicesList.Add(item.ServiceId.ToString());
            }
            ViewBag.Services = _serviceService.GetList(s =>
                !s.IsDeleted && s.IsActive && s.CreatedBy == SessionData.Current.User.Id);

            ViewBag.SelectedService = string.Join(",", servicesList);
            #endregion

            return View(new DepartmentEditCommand
            {
                Id = department.Id,
                Title = department.Title,
                Slogan = department.Slogan,
                Color = department.Color,
                AttachmentImageId = department.AttachmentImageId,
                Source = _attachmentFileService.GetAttachmentSourceValue(department.AttachmentImageId),
                IsActive = department.IsActive,
                DepartmentBackgroundHeaderAttachmentImageId = department.DepartmentBackgroundHeaderAttachmentImageId,
                JoinOurTeamImageAttachmentId = department.JoinOurTeamImageAttachmentId,
                JoinOurTeamDescription = department.JoinOurTeamDescription,
                StructureAttachmentImageId = department.StructureAttachmentImageId,
                InternshipButtonIsActive = department.InternshipButtonIsActive,
                RecruitmentButtonIsActive = department.RecruitmentButtonIsActive,
                StructureAttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(department.StructureAttachmentImageId),
                DepartmentBackgroundHeaderAttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(department.DepartmentBackgroundHeaderAttachmentImageId),
                JoinOurTeamImageSource = _attachmentFileService.GetAttachmentSourceValue(department.JoinOurTeamImageAttachmentId),
                ContactUsLink = department.ContactUsLink
            });
        }

        /// <summary>
        /// ویرایش دپارتمان
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <param name="headerBackgroundImage"></param>
        /// <param name="structureImage"></param>
        /// <param name="joinOurTeamImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentEditCommand command, HttpPostedFileBase file, HttpPostedFileBase headerBackgroundImage, HttpPostedFileBase structureImage, HttpPostedFileBase joinOurTeamImage)
        {
            var department = _departmentService.Get(command.Id).MapToEntity();
            if (department == null)
                return RedirectToAction("Index");

            var existsDepartment = _departmentService.Any(d => d.Title == command.Title && d.Id != department.Id);
            if (existsDepartment)
            {
                ModelState.AddModelError("Title", Strings.Exists_Title_Department);
                return View(command);
            }

            if (ModelState.IsValid)
            {
                #region Insert image in attachment file
                if (file != null && file.ContentLength > 0)
                {
                    if (department.AttachmentImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(department.AttachmentImageId.Value);

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
                        imageWidth, imageHeight, "DepartmentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.AttachmentImageId = attachmentId;
                }

                if (headerBackgroundImage != null && headerBackgroundImage.ContentLength > 0)
                {
                    if (command.DepartmentBackgroundHeaderAttachmentImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(command.DepartmentBackgroundHeaderAttachmentImageId.Value);

                    byte[] imageData;
                    var fileSize = headerBackgroundImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(headerBackgroundImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(headerBackgroundImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(headerBackgroundImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(headerBackgroundImage.FileName),
                        System.IO.Path.GetExtension(headerBackgroundImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "DepartmentHeaderBackgroundImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.DepartmentBackgroundHeaderAttachmentImageId = attachmentId;
                }

                if (structureImage != null && structureImage.ContentLength > 0)
                {
                    if (command.StructureAttachmentImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(command.StructureAttachmentImageId.Value);

                    byte[] imageData;
                    var fileSize = structureImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(structureImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(structureImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(structureImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(structureImage.FileName),
                        System.IO.Path.GetExtension(structureImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "StructureImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.StructureAttachmentImageId = attachmentId;
                }

                if (joinOurTeamImage != null && joinOurTeamImage.ContentLength > 0)
                {
                    if (command.JoinOurTeamImageAttachmentId.HasValue)
                        _attachmentFileService.RemoveAttachment(command.JoinOurTeamImageAttachmentId.Value);

                    byte[] imageData;
                    var fileSize = joinOurTeamImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(joinOurTeamImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(joinOurTeamImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(joinOurTeamImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(joinOurTeamImage.FileName),
                        System.IO.Path.GetExtension(joinOurTeamImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "JoinOurTeamImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.JoinOurTeamImageAttachmentId = attachmentId;
                }
                #endregion

                _departmentService.UpdateDepartment(department, command, SessionData.Current.User.Id);

                #region Assign service to department
                command.ServicesId?.RemoveAll(s => s == Guid.Empty);
                _departmentServiceService.AssignServiceToDepartment(department.Id, command.ServicesId, SessionData.Current.User.Id);
                #endregion

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "DepartmentsController", "Edit", "Success Edit Department", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            ViewBag.Services = _serviceService.GetList(s =>
                !s.IsDeleted && s.IsActive && s.CreatedBy == SessionData.Current.User.Id);

            #region Fill services

            List<string> servicesList = new List<string>();
            foreach (var item in department.DepartmentServices)
            {
                servicesList.Add(item.ServiceId.ToString());
            }
            ViewBag.SelectedService = string.Join(",", servicesList);
            #endregion

            return View(command);
        }

        /// <summary>
        /// تغییر دادن وضعیت فعال بودن دپارتمان
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var department = _departmentService.Get(id).MapToEntity();
            if (department == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            department.IsActive = !department.IsActive;
            _departmentService.Update(department);
            _departmentService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف دپارتمانس
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var department = _departmentService.Get(id);


                #region Remmove dependencies

                if (department.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(department.AttachmentImageId.Value);


                var departmentServices = _departmentServiceService.GetList(d => d.DepartmentId == department.Id);
                foreach (var departmentService in departmentServices)
                {
                    _departmentServiceService.Delete(departmentService.Id);
                }
                _departmentServiceService.Save();

                var employees = _employeeService.GetList(e => e.DepartmentId == department.Id);
                foreach (var employee in employees)
                {
                    _employeeService.Delete(employee.Id);
                }
                _employeeService.Save();

                var departmentSelectionReasons =
                    _departmentSelectionReasonService.GetList(d => d.DepartmentId == department.Id);
                foreach (var departmentSelectionReason in departmentSelectionReasons)
                {
                    _departmentSelectionReasonService.Delete(departmentSelectionReason.Id);
                }
                _departmentSelectionReasonService.Save();

                var departmentMethodologies = _methodologyService.GetList(m => m.DepartmentId == department.Id);
                foreach (var departmentMethodology in departmentMethodologies)
                {
                    _methodologyService.Delete(departmentMethodology.Id);
                }
                _methodologyService.Save();

                #endregion

                _departmentService.Delete(department.Id);
                _departmentService.Save();

                return Json(new
                {
                    Message = Strings.Departmen_Delete_Successfully,
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