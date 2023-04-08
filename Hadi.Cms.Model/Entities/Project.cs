using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// پروژه
    /// </summary>
    public class Project : BaseModel
    {
        public Project()
        {
            ProjectTags = new HashSet<ProjectTag>();
            ChallengeProjects = new HashSet<ChallengeProject>();
            ProjectTechnologies = new HashSet<ProjectTechnology>();
            ProjectAttachmentFiles = new HashSet<ProjectAttachmentFile>();
        }

        /// <summary>
        /// شناسه کارفرمای پروژه
        /// </summary>
        public Guid EmployerId { get; set; }
        /// <summary>
        /// عنوان پروژه
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// نوع پروژه
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// تصویر پیش نمایش
        /// </summary>

        public Guid? PreviewImageGuid { get; set; }
        /// <summary>
        /// شناسه عکس پروژه
        /// </summary>
        public Guid? ImageGuid { get; set; }
        /// <summary>
        /// عکس بخش اول
        /// </summary>
        public Guid? FirstStepImage { get; set; }
        /// <summary>
        /// توضیحات بخش اول
        /// </summary>
        [AllowHtml]
        public string FirstStepDescription { get; set; }
        /// <summary>
        /// توضیحات بخش دوم
        /// </summary>
        [AllowHtml]
        public string SecondStepDescription { get; set; }
        /// <summary>
        /// کلام آخر
        [AllowHtml]
        public string FinalStepDescription { get; set; }
        /// <summary>
        /// لینک پروژه
        /// </summary>
        public string ProjectLink { get; set; }
        /// <summary>
        /// متن لینک پروژه
        /// </summary>
        public string ProjectLinkText { get; set; }
        /// <summary>
        /// توضیحات در مورد پروژه
        /// </summary>
        [AllowHtml]
        public string Description { get; set; }

        public ICollection<ProjectTag> ProjectTags { get; set; }
        public ICollection<ChallengeProject> ChallengeProjects { get; set; }
        public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }
        public ICollection<ProjectAttachmentFile> ProjectAttachmentFiles { get; set; }
    }
}
