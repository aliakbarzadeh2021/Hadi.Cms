using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// فرصت های شغلی
    /// </summary>
    public class CareerOpportunity : BaseModel
    {
        public CareerOpportunity()
        {
            Resumes = new HashSet<Resume>();
        }

        /// <summary>
        /// عنوان شغل
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// نوع شغل
        /// </summary>
        public CareerOpportunityType CareerOpportunityType { get; set; }
        /// <summary>
        /// توضیحات شغل
        /// </summary>
        [AllowHtml]
        public string Description { get; set; }
        /// <summary>
        /// شناسه تصویر
        /// </summary>

        public Guid? CareerOpportunityImageGuid { get; set; }

        public ICollection<Resume> Resumes { get; set; }
    }
}
