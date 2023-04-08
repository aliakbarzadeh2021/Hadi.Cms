using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// برچسب
    /// </summary>
    public class Tag : BaseModel
    {
        public Tag()
        {
            ArticleTags = new HashSet<ArticleTag>();
            AttachmentFileTags = new HashSet<AttachmentFileTag>();
            ProjectTags = new HashSet<ProjectTag>();
            CourseTags = new HashSet<CourseTag>();
            ServiceTags = new HashSet<ServiceTag>();
        }
        /// <summary>
        /// عنوان برچسب
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// مقدار یکتا
        /// </summary>
        public string UniqueValue { get; set; }
        /// <summary>
        /// شناسه والد برچسب
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// رنگ
        /// </summary>
        public string Color { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }
        public ICollection<AttachmentFileTag> AttachmentFileTags { get; set; }
        public ICollection<ProjectTag> ProjectTags { get; set; }
        public ICollection<CourseTag> CourseTags { get; set; }
        public ICollection<ServiceTag> ServiceTags { get; set; }
    }
}
