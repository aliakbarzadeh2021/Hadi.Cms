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
    /// مدیریت مدرسین
    /// </summary>
    public class TeachersController : Controller
    {
        private readonly TeacherService _teacherService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly CourseTeacherService _courseTeacherService;

        public TeachersController()
        {
            _teacherService = new TeacherService();
            _eventLoger = new EventLoger();
            _attachmentFileService = new AttachmentFileService();
            _courseTeacherService = new CourseTeacherService();
        }

        /// <summary>
        /// دریافت لیست مدرسین
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var teachers = _teacherService.GetList(t => !t.IsDeleted, o => o.OrderByDescending(t => t.CreatedDate), t => t.CourseTeachers);
            foreach (var teacher in teachers)
                teacher.AttachmentImageSource = teacher.AttachmentImageId.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(teacher.AttachmentImageId) : "/PhysicalStorage/no_image.png";

            var pagedList = teachers.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت مدرس
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new TeacherCreateCommand());
        }

        /// <summary>
        /// ثبت مدرس جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <param name="socialNetworkImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeacherCreateCommand command, HttpPostedFileBase file, HttpPostedFileBase socialNetworkImage)
        {
            if (!ModelState.IsValid)
                return View(command);

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
                    imageWidth, imageHeight, "TeacherImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            if (socialNetworkImage != null && socialNetworkImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = socialNetworkImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(socialNetworkImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(socialNetworkImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(socialNetworkImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(socialNetworkImage.FileName),
                    System.IO.Path.GetExtension(socialNetworkImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "TeacherSocialNetworkImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SocialNetworkImageGuid = attachmentId;
            }
            #endregion

            _teacherService.CreateNewTeacher(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TeachersController", "Create", "Success Create Teacher", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش مدرس
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var teacher = _teacherService.Get(id).MapToEntity();
            if (teacher == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new TeacherEditCommand
            {
                FullName = teacher.FullName,
                AttachmentImageId = teacher.AttachmentImageId,
                AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(teacher.AttachmentImageId),
                SocialNetworkImageSource = _attachmentFileService.GetAttachmentSourceValue(teacher.SocialNetworkImageGuid),
                SocialNetworkName = teacher.SocialNetworkName,
                SocialNetworkLink = teacher.SocialNetworkLink,
                SocialNetworkImageGuid = teacher.SocialNetworkImageGuid,
                IsActive = teacher.IsActive
            });
        }

        /// <summary>
        /// ویرایش مدرس
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <param name="socialNetworkImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeacherEditCommand command, HttpPostedFileBase file, HttpPostedFileBase socialNetworkImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            var teacher = _teacherService.Get(command.Id).MapToEntity();
            if (teacher == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            #region Insert image in attachment file .
            if (file != null && file.ContentLength > 0)
            {
                if (teacher.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(teacher.AttachmentImageId.Value);

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
                    imageWidth, imageHeight, "TeacherImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AttachmentImageId = attachmentId;
            }

            if (socialNetworkImage != null && socialNetworkImage.ContentLength > 0)
            {
                if (teacher.SocialNetworkImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(teacher.SocialNetworkImageGuid.Value);

                byte[] imageData;
                var fileSize = socialNetworkImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(socialNetworkImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(socialNetworkImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(socialNetworkImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(socialNetworkImage.FileName),
                    System.IO.Path.GetExtension(socialNetworkImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "TeacherSocialNetworkImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.SocialNetworkImageGuid = attachmentId;
            }
            #endregion

            _teacherService.UpdateTeacher(teacher, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TeachersController", "Edit", "Success Edit Teacher", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر دادن وضعیت فعال بودن مدرس
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var teacher = _teacherService.Get(id).MapToEntity();
            if (teacher == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            teacher.IsActive = !teacher.IsActive;
            _teacherService.Update(teacher);
            _teacherService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف مدرس
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var teacher = _teacherService.Get(id).MapToEntity();

                #region Remove dependencies

                if (teacher.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(teacher.AttachmentImageId.Value);

                var courseTeacherList = _courseTeacherService.GetList(c => c.TeacherId == teacher.Id);
                foreach (var courseTeacher in courseTeacherList)
                {
                    _courseTeacherService.Delete(courseTeacher.Id);
                }
                _courseTeacherService.Save();

                #endregion

                _teacherService.Delete(teacher.Id);
                _teacherService.Save();
                
                return Json(new
                {
                    Message = Strings.Teacher_Delete_Successfully,
                    Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Strings.Error,
                    Type = "error"
                });
            }
        }
    }
}