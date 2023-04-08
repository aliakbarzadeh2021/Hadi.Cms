using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// چالش
    /// </summary>
    public class Challenge : BaseModel
    {
        public Challenge()
        {
            ChallengeProjects = new HashSet<ChallengeProject>();
        }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [AllowHtml]
        public string Description { get; set; }
        /// <summary>
        /// شرح مسئله
        /// </summary>
        [AllowHtml]
        public string ProblemDescription { get; set; }
        /// <summary>
        /// راه حل مسئله
        /// </summary>
        [AllowHtml]
        public string ProblemSolvingDescription { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? ImageAttachmentId { get; set; }
        /// <summary>
        /// شناسه ویدیو ضمیمه شده
        /// </summary>
        public Guid? VideoAttachmentId { get; set; }
        public ICollection<ChallengeProject> ChallengeProjects { get; set; }
    }
}
