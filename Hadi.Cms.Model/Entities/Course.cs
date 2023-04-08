using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دوره ها
    /// </summary>
    public class Course : BaseModel
    {
        public Course()
        {
            CourseTeachers = new HashSet<CourseTeacher>();
            CourseAttachmentFiles = new HashSet<CourseAttachmentFile>();
            CourseTags = new HashSet<CourseTag>();
        }
        /// <summary>
        /// عنوان دوره
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// مدت زمان دوره
        /// </summary>
        public string PeriodOfTime { get; set; }
        /// <summary>
        /// قیمت
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// شناسه ویدیو پیش نمایش
        /// </summary>
        public Guid? AttachmentCoursePreviewVideoId { get; set; }
        /// <summary>
        /// شناسه عکس پستر ویدیو پیش نمایش
        /// </summary>
        public Guid? CoursePreviewVideoPosterImageId { get; set; }
        /// <summary>
        /// شناسه تصویر دوره
        /// </summary>
        public Guid? AttachmentImageId { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [AllowHtml]
        public string Description { get; set; }

        public ICollection<CourseTeacher> CourseTeachers { get; set; }
        public ICollection<CourseAttachmentFile> CourseAttachmentFiles { get; set; }
        public ICollection<CourseTag> CourseTags { get; set; }
    }
}
