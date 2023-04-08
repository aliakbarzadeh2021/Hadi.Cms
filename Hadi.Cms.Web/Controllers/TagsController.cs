using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Enums;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// برچسب ها
    /// </summary>
    public class TagsController : Controller
    {
        private readonly TagService _tagService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly CourseService _courseService;

        public TagsController()
        {
            _tagService = new TagService();
            _attachmentFileService = new AttachmentFileService();
            _courseService = new CourseService();
        }

        [ChildActionOnly]
        public ActionResult TagsPartial(/*bool showAll = false*/)
        {

            var tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId != null);
            tags = tags.OrderBy(o => o.OrderId).ToList();
            return PartialView("_TagsPartial", tags);
        }

        /// <summary>
        /// دریاف تگ های بیشتر
        /// </summary>
        /// <param name="count"></param>
        /// <param name="id"></param>
        /// <returns></returns> 
        public ActionResult GetMoreTags(int count = 1, Guid? id = null)
        {
            var tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId != null);
            tags = tags.OrderBy(o => o.OrderId).Take(7 * count).ToList();

            return Json(new
            {
                Tags = tags.Select(t => new
                {
                    Id = t.Id,
                    Title = t.Title,
                }).ToList(),
                SelectedTagId = id,
                CountOfTags = tags.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// دریافت ویدویوهای تگ بر اساس شناسه تگ
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public ActionResult GetTagVideosByTagId(Guid? tagId)
        {
            var courses = new List<ICourseDto>();

            if (tagId != null)
            {
                var tag = _tagService.Get(t => t.Id == tagId);

                if (tag != null)
                {
                    var relatedCourses = _courseService.GetList(c => c.CourseTags.Any(ct => ct.TagId == tagId));
                    courses.AddRange(relatedCourses);
                }

                courses = courses.DistinctBy(c => c.Id).ToList().Where(c => c.AttachmentCoursePreviewVideoId != null).Take(3).ToList();
            }

            else
            {
                var relatedCourses = _courseService
                    .GetList(c => c.IsActive && !c.IsDeleted && c.AttachmentCoursePreviewVideoId != null, o => o.OrderByDescending(c => c.CreatedDate)).Take(3)
                    .ToList();

                foreach (var relatedCourse in relatedCourses)
                    courses.Add(relatedCourse);
            }

            return Json(new
            {
                CourseVideos = courses.Select(c => new
                {
                    CourseVideoSource = _attachmentFileService.GetAttachmentSourceValue(c.AttachmentCoursePreviewVideoId),
                    CourseVideoPosterSource = _attachmentFileService.GetAttachmentSourceValue(c.CoursePreviewVideoPosterImageId)
                }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}