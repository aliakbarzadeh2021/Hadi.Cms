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
    /// مدیریت کارمندان
    /// </summary>
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public EmployeeController()
        {
            _employeeService = new EmployeeService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست کارمندان
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var employees = _employeeService.GetList(e => !e.IsDeleted && e.DepartmentId == id && e.CreatedBy == SessionData.Current.User.Id);
            foreach (var employee in employees)
            {
                employee.ImageSource = employee.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(employee.AttachmentImageId) : "/PhysicalStorage/no_image.png";
                employee.FullName = $"{employee.FirstName} {employee.LastName}";
            }

            var pagedList = employees.OrderBy(e => e.OrderId).ThenBy(e => e.LastName).ToPagedList(pageNumber, pageSize);

            ViewBag.DepartmentId = id;
            ViewBag.PageNumber = pageNumber;

            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");
            TempData["Id"] = id;
            TempData.Keep("Id");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت کارمند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            return View(new EmployeeCreateCommand()
            {
                DepartmentId = id
            });
        }

        /// <summary>
        /// ثبت  کارمند جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            #region Insert image in attachemnt file

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
                    imageWidth, imageHeight, "EmployeeImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            _employeeService.CreateNewEmployee(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "EmployeeController", "Create", "Success Create Employee", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ویرایش کارمند
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id, Guid departmentId)
        {
            var employee = _employeeService.Get(id);
            if (employee == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new EmployeeEditCommand
            {
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                RoleTitle = employee.RoleTitle,
                OrderId = employee.OrderId,
                AttachmentImageId = employee.AttachmentImageId,
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(employee.AttachmentImageId),
                IsActive = employee.IsActive
            });
        }

        /// <summary>
        /// ویرایش کارمند
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEditCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var employee = _employeeService.Get(e => e.Id == command.Id).MapToEntity();
            if (employee == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Insert image in attachemnt file

            if (file != null && file.ContentLength > 0)
            {
                if (employee.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(employee.AttachmentImageId.Value);

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
                    imageWidth, imageHeight, "EmployeeImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }
            #endregion

            _employeeService.UpdateEmployee(employee, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "EmployeeController", "Edit", "Success Edit Employee", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن کارمند
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id, Guid departmentId)
        {
            var employee = _employeeService.Get(id).MapToEntity();
            if (employee == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });


            employee.IsActive = !employee.IsActive;
            _employeeService.Update(employee);
            _employeeService.Save();
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف کارمند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var employee = _employeeService.Get(id).MapToEntity();

                #region Remove attachment image
                if (employee.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(employee.AttachmentImageId.Value);
                #endregion

                _employeeService.Delete(employee.Id);
                _employeeService.Save();

                return Json(new
                {
                    Message = Strings.Employee_Delete_Successfully,
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