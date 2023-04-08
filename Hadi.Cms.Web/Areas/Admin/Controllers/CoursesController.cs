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
    /// مدیریت دوره ها
    /// </summary>
    public class CoursesController : Controller
    {
        private readonly CourseService _courseService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly TeacherService _teacherService;
        private readonly TagService _tagService;
        private readonly CourseAttachmentFileService _courseAttachmentFileService;
        private readonly CourseTeacherService _courseTeacherService;
        private readonly CourseTagService _courseTagService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileTagService _attachmentFileTagService;
        public CoursesController()
        {
            _courseService = new CourseService();
            _attachmentFileService = new AttachmentFileService();
            _teacherService = new TeacherService();
            _tagService = new TagService();
            _courseAttachmentFileService = new CourseAttachmentFileService();
            _courseTeacherService = new CourseTeacherService();
            _courseTagService = new CourseTagService();
            _eventLoger = new EventLoger();
            _attachmentFileTagService = new AttachmentFileTagService();
        }

        /// <summary>
        /// دریافت لیست دوره ها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var courses = _courseService.GetList(c => !c.IsDeleted && c.CreatedBy == SessionData.Current.User.Id, null,
                c => c.CourseTags, c => c.CourseTags.Select(ct => ct.Tag));
            
            foreach (var course in courses)
            {
                course.AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(course.AttachmentImageId);
                course.PriceString = course.Price == 0 ? "رایگان" : course.Price.ToString("##,000 تومان");
                course.AttachmentImageSource = course.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(course.AttachmentImageId) : "/PhysicalStorage/no_image.png";
            }

            var pagedList = courses.OrderByDescending(c => c.CreatedDate).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت دوره
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var userId = SessionData.Current.User.Id;

            ViewBag.Teachers = _teacherService.GetList(t =>
                t.IsActive && !t.IsDeleted && t.CreatedBy == userId);

            ViewBag.Tags =
        _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == userId && t.ParentId != null);

            return View(new CourseCreateCommand());
        }

        /// <summary>
        /// ثبت دوره جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="coursePreviewVideo"></param>
        /// <param name="coursePreviewVideoPosterImage"></param>
        /// <param name="courseVideos"></param>
        /// <param name="courseImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseCreateCommand command, HttpPostedFileBase coursePreviewVideo,
            HttpPostedFileBase coursePreviewVideoPosterImage, HttpPostedFileBase courseImage, List<HttpPostedFileBase> courseVideos)

        {
            if (ModelState.IsValid)
            {
                var existsCourse = _courseService.Any(c => c.Title == command.Title);
                if (existsCourse)
                {
                    var userId = SessionData.Current.User.Id;

                    ViewBag.Teachers = _teacherService.GetList(t =>
                        t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id);
                    ViewBag.Tags =
                        _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == userId && t.ParentId != null);


                    ModelState.AddModelError("Title", Strings.Course_Title_Exists);
                    return View(command);
                }

                #region Insert course image in attachment file .

                if (courseImage != null && courseImage.ContentLength > 0)
                {
                    byte[] imageData;
                    var fileSize = courseImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(courseImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(courseImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(courseImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(courseImage.FileName),
                        System.IO.Path.GetExtension(courseImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "CourseImageId - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.AttachmentImageId = attachmentId;
                }
                #endregion

                #region Insert preview video in attachment file .

                if (coursePreviewVideo != null && coursePreviewVideo.ContentLength > 0)
                {
                    byte[] videoData;
                    var fileSize = coursePreviewVideo.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(coursePreviewVideo.InputStream))
                    {
                        videoData = binaryReader.ReadBytes(coursePreviewVideo.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(coursePreviewVideo.FileName));

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(coursePreviewVideo.FileName),
                        System.IO.Path.GetExtension(coursePreviewVideo.FileName), fileSize, mimeType,
                        0, 0, "CoursePreviewVideo - " + Guid.NewGuid(), "", videoData, DateTime.Now);

                    command.AttachmentCoursePreviewVideoId = attachmentId;
                }
                #endregion

                #region Insert preview video poster image in attachment file .

                if (coursePreviewVideoPosterImage != null && coursePreviewVideoPosterImage.ContentLength > 0)
                {
                    byte[] imageData;
                    var fileSize = coursePreviewVideoPosterImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(coursePreviewVideoPosterImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(coursePreviewVideoPosterImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(coursePreviewVideoPosterImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(coursePreviewVideoPosterImage.FileName),
                        System.IO.Path.GetExtension(coursePreviewVideoPosterImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "CoursePreviewVideoPosterImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.CoursePreviewVideoPosterImageId = attachmentId;
                }
                #endregion

                #region Insert course video in course attachemnt file service .

                courseVideos?.RemoveAll(c => c == null);
                if (courseVideos != null && courseVideos.Count > 0)
                {
                    foreach (var video in courseVideos)
                    {
                        if (video != null && video.ContentLength > 0)
                        {
                            byte[] videoData;
                            var fileSize = video.ContentLength;
                            using (var binaryReader = new System.IO.BinaryReader(video.InputStream))
                            {
                                videoData = binaryReader.ReadBytes(video.ContentLength);
                            }

                            var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(video.FileName));

                            var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(video.FileName),
                                System.IO.Path.GetExtension(video.FileName), fileSize, mimeType,
                                0, 0, "CourseVideo - " + Guid.NewGuid(), "", videoData, DateTime.Now);

                            command.CourseVideosId.Add(attachmentId);
                        }
                    }
                }
                #endregion

                var newCourseId = _courseService.CreateNewCourse(command, SessionData.Current.User.Id);

                #region Assign videos to course
                if (command.CourseVideosId.Count > 0)
                    _courseAttachmentFileService.AssignVideosToCourse(newCourseId, command.CourseVideosId, SessionData.Current.User.Id);
                #endregion

                #region Assign teachers to course

                command.CourseTeachersId?.RemoveAll(c => c == Guid.Empty);
                _courseTeacherService.AssignTeachersToCourse(newCourseId, command.CourseTeachersId, SessionData.Current.User.Id);
                #endregion

                #region Assign tags to course
                command.CourseTagsId?.RemoveAll(c => c == Guid.Empty);
                _courseTagService.AssignTagsToCourse(newCourseId, command.CourseTagsId, SessionData.Current.User.Id);
                #endregion

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "CourseController", "Create", "Success Create Course", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index" ,  new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1});
            }

            ViewBag.Teachers = _teacherService.GetList(t =>
                t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Tags =
                _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);

            return View(command);
        }

        /// <summary>
        /// صفحه ی ویرایش دوره
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var course = _courseService.GetList(c => c.Id == id, null, c => c.CourseAttachmentFiles,
                c => c.CourseTeachers, c => c.CourseTags).FirstOrDefault();

            #region Fill tags             
            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null);
            List<string> tagsList = new List<string>();
            foreach (var item in course.CourseTagDto)
            {
                tagsList.Add(item.TagId.ToString());
            }
            ViewBag.SelectedTags = string.Join(",", tagsList);
            #endregion

            #region Fill teachers

            ViewBag.Teachers = _teacherService.GetList(q =>
                !q.IsDeleted && q.IsActive && q.CreatedBy == SessionData.Current.User.Id);
            List<string> teacherList = new List<string>();
            foreach (var item in course.CourseTeacherDto)
            {
                teacherList.Add(item.TeacherId.ToString());
            }
            ViewBag.SelectedTeachers = string.Join(",", teacherList);
            #endregion

            return View(new CourseEditCommand
            {
                Id = course.Id,
                Title = course.Title,
                PeriodOfTime = course.PeriodOfTime,
                Price = Convert.ToDouble(course.Price),
                IsActive = course.IsActive,
                AttachmentImageId = course.AttachmentImageId,
                AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(course.AttachmentImageId),
                AttachmentCoursePreviewVideoId = course.AttachmentCoursePreviewVideoId,
                AttachmentCoursePreviewVideoSource = _attachmentFileService.GetAttachmentSourceValue(course.AttachmentCoursePreviewVideoId),
                CoursePreviewVideoPosterImageId = course.CoursePreviewVideoPosterImageId,
                AttachmentCoursePreviewVideoPosterImageSource = _attachmentFileService.GetAttachmentSourceValue(course.CoursePreviewVideoPosterImageId),
                Description = course.Description
            });
        }

        /// <summary>
        /// ویرایش دوره
        /// </summary>
        /// <param name="command"></param>
        /// <param name="coursePreviewVideo"></param>
        /// <param name="coursePreviewVideoPosterImage"></param>
        /// <param name="courseImage"></param>
        /// <param name="courseVideos"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseEditCommand command, HttpPostedFileBase coursePreviewVideo,
            HttpPostedFileBase coursePreviewVideoPosterImage, HttpPostedFileBase courseImage,
            List<HttpPostedFileBase> courseVideos)
        {
            var course =
                _courseService.GetList(
                    c => c.Id == command.Id && !c.IsDeleted && c.CreatedBy == SessionData.Current.User.Id, null,
                    c => c.CourseTags, c => c.CourseTeachers).FirstOrDefault().MapToEntity();

            if (ModelState.IsValid)
            {
                var existsCourse = _courseService.Any(c => c.Title == command.Title && c.Id != command.Id);
                if (existsCourse)
                {
                    var userId = SessionData.Current.User.Id;

                    ViewBag.Teachers = _teacherService.GetList(t =>
                        t.IsActive && !t.IsDeleted && t.CreatedBy == userId);

                    ViewBag.Tags =
                        _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId != null && t.CreatedBy == userId);


                    ModelState.AddModelError("Title", Strings.Course_Title_Exists);
                    return View(command);
                }

                #region Insert course image in attachment file .

                if (courseImage != null && courseImage.ContentLength > 0)
                {
                    if (course.AttachmentImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(course.AttachmentImageId.Value);

                    byte[] imageData;
                    var fileSize = courseImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(courseImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(courseImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(courseImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(courseImage.FileName),
                        System.IO.Path.GetExtension(coursePreviewVideoPosterImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "CourseImageId - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.AttachmentImageId = attachmentId;
                }
                #endregion

                #region Insert preview video in attachment file .

                if (coursePreviewVideo != null && coursePreviewVideo.ContentLength > 0)
                {
                    if (course.AttachmentCoursePreviewVideoId.HasValue)
                        _attachmentFileService.RemoveAttachment(course.AttachmentCoursePreviewVideoId.Value);

                    byte[] videoData;
                    var fileSize = coursePreviewVideo.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(coursePreviewVideo.InputStream))
                    {
                        videoData = binaryReader.ReadBytes(coursePreviewVideo.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(coursePreviewVideo.FileName));

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(coursePreviewVideo.FileName),
                        System.IO.Path.GetExtension(coursePreviewVideo.FileName), fileSize, mimeType,
                        0, 0, "CoursePreviewVideo - " + Guid.NewGuid(), "", videoData, DateTime.Now);

                    command.AttachmentCoursePreviewVideoId = attachmentId;
                }
                #endregion

                #region Insert preview video poster image in attachment file .

                if (coursePreviewVideoPosterImage != null && coursePreviewVideoPosterImage.ContentLength > 0)
                {
                    if (course.CoursePreviewVideoPosterImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(course.CoursePreviewVideoPosterImageId.Value);

                    byte[] imageData;
                    var fileSize = coursePreviewVideoPosterImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(coursePreviewVideoPosterImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(coursePreviewVideoPosterImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(coursePreviewVideoPosterImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(coursePreviewVideoPosterImage.FileName),
                        System.IO.Path.GetExtension(coursePreviewVideoPosterImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "CoursePreviewVideoPosterImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.CoursePreviewVideoPosterImageId = attachmentId;
                }
                #endregion

                #region Insert course video in course attachemtn file service .

                if (courseVideos != null && courseVideos.Count > 0)
                {
                    foreach (var video in courseVideos)
                    {
                        if (video != null && video.ContentLength > 0)
                        {
                            byte[] videoData;
                            var fileSize = video.ContentLength;
                            using (var binaryReader = new System.IO.BinaryReader(video.InputStream))
                            {
                                videoData = binaryReader.ReadBytes(video.ContentLength);
                            }

                            var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(video.FileName));

                            var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(video.FileName),
                                System.IO.Path.GetExtension(video.FileName), fileSize, mimeType,
                                0, 0, "CourseVideo - " + Guid.NewGuid(), "", videoData, DateTime.Now);

                            command.CourseVideosId.Add(attachmentId);
                        }
                    }
                }
                #endregion

                _courseService.UpdateCourse(course, command, SessionData.Current.User.Id);

                #region Assign videos to course
                if (command.CourseVideosId.Count > 0)
                    _courseAttachmentFileService.AssignVideosToCourse(course.Id, command.CourseVideosId, SessionData.Current.User.Id);
                #endregion

                #region Assign teachers to course

                command.CourseTeachersId?.RemoveAll(c => c == Guid.Empty);
                _courseTeacherService.AssignTeachersToCourse(course.Id, command.CourseTeachersId, SessionData.Current.User.Id);
                #endregion

                #region Assign tags to course
                command.CourseTagsId?.RemoveAll(c => c == Guid.Empty);
                _courseTagService.AssignTagsToCourse(course.Id, command.CourseTagsId, SessionData.Current.User.Id);
                #endregion


                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "CourseController", "Edit", "Success Edit Course", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            #region Fill tags             
            ViewBag.Tags = _tagService.GetList(q => !q.IsDeleted && q.IsActive && q.ParentId != null && q.CreatedBy == SessionData.Current.User.Id);
            List<string> tagsList = new List<string>();
            foreach (var item in course.CourseTags)
            {
                tagsList.Add(item.TagId.ToString());
            }
            ViewBag.SelectedTags = string.Join(",", tagsList);
            #endregion

            #region Fill teachers             
            ViewBag.Teachers = _teacherService.GetList(q => !q.IsDeleted && q.IsActive && q.CreatedBy == SessionData.Current.User.Id);
            List<string> teacherList = new List<string>();
            foreach (var item in course.CourseTeachers)
            {
                teacherList.Add(item.TeacherId.ToString());
            }
            ViewBag.SelectedTeachers = string.Join(",", teacherList);
            #endregion

            return View(command);
        }

        /// <summary>
        /// تغییر دادن وضعیت فعال بودن دوره
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var course = _courseService.Get(id).MapToEntity();
            if (course == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            course.IsActive = !course.IsActive;
            _courseService.Update(course);
            _courseService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف یک دوره
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var course = _courseService.Get(id).MapToEntity();

                #region Remove dependencies

                #region Remove course attachment image .
                if (course.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(course.AttachmentImageId.Value);
                #endregion

                #region Remove course preview video

                if (course.AttachmentCoursePreviewVideoId.HasValue)
                    _attachmentFileService.RemoveAttachment(course.AttachmentCoursePreviewVideoId.Value);

                #endregion

                #region Remove course preview video poster image .
                if (course.CoursePreviewVideoPosterImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(course.CoursePreviewVideoPosterImageId.Value);
                #endregion

                #region Remove course tags

                var courseTags = _courseTagService.GetList(c => c.CourseId == course.Id);
                foreach (var courseCourseCategory in courseTags)
                {
                    _courseTagService.Delete(courseCourseCategory.Id);
                }
                _courseTagService.Save();
                #endregion

                #region Remove course teachers

                var courseTeachers = _courseTeacherService.GetList(c => c.CourseId == course.Id);
                foreach (var courseTeacher in courseTeachers)
                {
                    _courseTeacherService.Delete(courseTeacher.Id);
                }
                _courseTeacherService.Save();
                #endregion

                #region Remove course videos

                var courseAttachmentFiles = _courseAttachmentFileService.GetList(c => c.CourseId == course.Id);
                foreach (var courseAttachmentFile in courseAttachmentFiles)
                {
                    _courseAttachmentFileService.Delete(courseAttachmentFile.Id);
                }
                _courseAttachmentFileService.Save();


                #endregion

                #endregion

                _courseService.Delete(course.Id);
                _courseService.Save();

                return Json(new
                {
                    Message = Strings.Delete_Course_Successfully,
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