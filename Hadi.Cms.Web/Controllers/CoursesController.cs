using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Web.Mvc.Razor;
using System.Web.Mvc.Html;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// دوره
    /// </summary>
    public class CoursesController : Controller
    {
        private readonly CourseService _courseService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ServiceService _serviceService;
        private readonly ProjectService _projectService;
        private readonly ArticleService _articleService;
        private readonly UserService _userService;
        private readonly EventService _eventService;
        private readonly TagService _tagService;
        private readonly AuthorService _authorService;
        private readonly TeacherService _teacherService;
        public CoursesController()
        {
            _courseService = new CourseService();
            _attachmentFileService = new AttachmentFileService();
            _serviceService = new ServiceService();
            _articleService = new ArticleService();
            _userService = new UserService();
            _projectService = new ProjectService();
            _eventService = new EventService();
            _tagService = new TagService();
            _authorService = new AuthorService();
            _teacherService = new TeacherService();
        }

        public ActionResult Index()
        {
            var services = _serviceService.GetList(s => s.IsActive && !s.IsDeleted, s => s.OrderByDescending(o => o.CreatedDate), s => s.ServiceTags, s => s.ServiceTags.Select(st => st.Tag));

            var dto = new AllCoursesDto();

            dto.Event = _eventService.GetList(e => e.IsActive && !e.IsDeleted).OrderByDescending(e => e.CreatedDate).FirstOrDefault();
            if (dto.Event != null)
                dto.Event.AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(dto.Event.AttachmentImageId);

            foreach (var service in services)
            {
                var allCoursesPageDetail = new AllCoursePageDetailDto();

                service.Title = service.Title.Replace("<br/>", " ");
                allCoursesPageDetail.Service = service;
                var tags = service.ServiceTagDto.Select(s => s.TagDto).ToList();

                foreach (var tag in tags)
                {
                    var parentTag = _tagService.Get(t => t.IsActive && !t.IsDeleted && t.Id == tag.ParentId);
                    if (parentTag != null)
                    {
                        var childrenTags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId == parentTag.Id);

                        allCoursesPageDetail.Tags = childrenTags;

                        foreach (var childTag in childrenTags)
                        {
                            #region Get courses list

                            var courses = _courseService.GetList(c => c.IsActive && !c.IsDeleted && c.CourseTags.Any(ct => ct.TagId == childTag.Id), null, c => c.CourseTags, c => c.CourseAttachmentFiles,
                            c => c.CourseAttachmentFiles.Select(ca => ca.AttachmentFile),
                            c => c.CourseTags.Select(ct => ct.Tag), c => c.CourseTeachers,
                             c => c.CourseTeachers.Select(ct => ct.Teacher));

                            #endregion

                            #region Get articles list

                            var articles = _articleService.GetList(a => a.IsActive && !a.IsDeleted && a.ArticleTags.Any(at => at.TagId == childTag.Id), null, a => a.ArticleTags, a => a.ArticleTags.Select(at => at.Tag));

                            #endregion

                            #region Get challenges list

                            var challenges = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.ProjectTags.Any(pt => pt.TagId == childTag.Id), null, p => p.ChallengeProjects,
                               p => p.ChallengeProjects.Select(cp => cp.Challenge)).SelectMany(p => p.ChallengeProject).Select(c => c.Challenge).ToList();

                            #endregion

                            #region Get attachment videos list

                            var attachmentFiles = _attachmentFileService.GetList(a => a.AttachmentFileTags.Any(at => at.TagId == childTag.Id), null, a => a.AttachmentFileTags);

                            #endregion

                            #region Map to dto

                            allCoursesPageDetail.Challenges.AddRange(challenges);
                            allCoursesPageDetail.Teachers.AddRange(courses.SelectMany(c => c.CourseTeacherDto, (c, ct) => ct.TeacherDto).ToList());
                            allCoursesPageDetail.Courses.AddRange(courses);
                            allCoursesPageDetail.Articles.AddRange(articles.Select(a => new AllArticleDto()
                            {
                                Id = a.Id,
                                Title = a.Title,
                                ArticleImageGuid = a.AttachmentImageId,
                                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.AttachmentImageId),
                                AuthorId = a.AuthorId,
                                AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                                AuthorImageGuid = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid : null,
                                AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                                Tags = a.ArticleTagsDto.Select(at => at.TagDto).ToList(),
                            }).OrderByDescending(a => a.CreatedDate).Take(4).ToList());
                            allCoursesPageDetail.AttachmentVideos.AddRange(attachmentFiles.Select(a => new AttachmentVideoDto()
                            {
                                Id = a.Id,
                                MimeType = a.MimeType,
                                VideoSource = _attachmentFileService.GetAttachmentSourceValue(a.Id),
                                VideoPosterSource = _attachmentFileService.GetAttachmentSourceValue(a.PosterImageGuid)
                            }).ToList());

                            #endregion
                        }
                    }
                }

                #region Remove duplicates record from lists

                allCoursesPageDetail.Articles = allCoursesPageDetail.Articles.DistinctBy(a => a.Id).OrderByDescending(a => a.CreatedDate).Take(4).ToList();
                allCoursesPageDetail.AttachmentVideos = allCoursesPageDetail.AttachmentVideos.DistinctBy(a => a.Id).ToList();
                allCoursesPageDetail.Challenges = allCoursesPageDetail.Challenges.DistinctBy(c => c.Id).OrderByDescending(a => a.CreatedDate).Take(4).ToList();
                allCoursesPageDetail.Courses = allCoursesPageDetail.Courses.DistinctBy(c => c.Id).OrderByDescending(a => a.CreatedDate).ToList();
                allCoursesPageDetail.Tags = allCoursesPageDetail.Tags.DistinctBy(t => t.Id).OrderByDescending(a => a.CreatedDate).ToList();
                allCoursesPageDetail.Teachers = allCoursesPageDetail.Teachers.DistinctBy(t => t.Id).OrderByDescending(a => a.CreatedDate).ToList();

                #endregion

                foreach (var course in allCoursesPageDetail.Courses)
                {
                    course.AttachmentImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(course.AttachmentImageId);

                    course.PriceString = course.Price == 0 ? "رایگان" : course.Price.ToString("##,000 تومان");
                }

                foreach (var challenge in allCoursesPageDetail.Challenges)
                {
                    challenge.ImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(challenge.ImageAttachmentId);

                    challenge.VideoSource =
                        _attachmentFileService.GetAttachmentSourceValue(challenge.VideoAttachmentId);

                    challenge.ProjectName = string.Join(" - ", challenge.ChallengeProjects.Select(c => c.Project.Title).ToList());
                }

                foreach (var teacher in allCoursesPageDetail.Teachers)
                {
                    teacher.AttachmentImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(teacher.AttachmentImageId);

                    teacher.SocialNetworkImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(teacher.SocialNetworkImageGuid);
                }

                dto.AllCoursePageDetails.Add(allCoursesPageDetail);
            }

            return View(dto);
        }

        /// <summary>
        /// جزئیات دوره
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public ActionResult Details(Guid id, Guid serviceId)
        {
            var course = _courseService.GetList(c => c.IsActive && !c.IsDeleted && c.Id == id, null,
                c => c.CourseTeachers, c => c.CourseTeachers.Select(cc => cc.Teacher) , c => c.CourseTags , c => c.CourseTags.Select(ct => ct.Tag)).FirstOrDefault();

            if (course == null)
                return RedirectToAction("Index");

            course.AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(course.AttachmentImageId);

            course.AttachmentCoursePreviewVideoSource =
                _attachmentFileService.GetAttachmentSourceValue(course.AttachmentCoursePreviewVideoId);

            course.CoursePreviewVideoPosterImageSource =
                _attachmentFileService.GetAttachmentSourceValue(course.CoursePreviewVideoPosterImageId);

            foreach (var courseTeacher in course.CourseTeacherDto)
                courseTeacher.TeacherDto.AttachmentImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(courseTeacher.TeacherDto.AttachmentImageId);

            course.PriceString = course.Price == 0 ? "رایگان" : course.Price.ToString("##,000 تومان");

            var service = _serviceService.GetList(s => s.Id == serviceId, null, s => s.DepartmentServices,
                s => s.DepartmentServices.Select(d => d.Department)).FirstOrDefault();

            ViewBag.CourseTitle = course.Title;
            ViewBag.CourseDescription = course.Description;
            ViewBag.CourseImage = course.AttachmentImageSource;
            ViewBag.WriteMetaTag = true;

            if (service != null)
                ViewBag.Departments = service.DepartmentServicesDto.Select(ds => ds.DepartmentDto).ToList();

            return View(course);
        }


        /// <summary>
        /// دریافت ویدیو بر اساس تگ
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public ActionResult GetVideosByTagId(Guid tagId)
        {
            var attachmentFiles = new List<IAttachmentFileDto>();

            var videoExtensions = new[] { ".3gp",".mkv",".mp4"};

            var tag = _tagService.Get(t => t.Id == tagId);

            if (tag != null)
            {
                attachmentFiles.AddRange(_attachmentFileService.GetList(a =>
                    videoExtensions.Any(v => a.Extension == v) && a.AttachmentFileTags.Any(at => at.TagId == tag.Id)));
                attachmentFiles = attachmentFiles.DistinctBy(a => a.Id).ToList();
            }


            return Json(new
            {
                AttachmentFiles = attachmentFiles.Select(a => new
                {
                    VideoSource = _attachmentFileService.GetAttachmentSourceValue(a.Id),
                    VideoPosterImageSource = _attachmentFileService.GetAttachmentSourceValue(a.PosterImageGuid),
                }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}