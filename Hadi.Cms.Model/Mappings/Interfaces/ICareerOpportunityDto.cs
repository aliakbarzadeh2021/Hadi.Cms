using System;
using System.Collections.Generic;
using System.Web.Caching;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ICareerOpportunityDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// عنوان شغل
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// نوع شغل
        /// </summary>
         CareerOpportunityType CareerOpportunityType { get; set; }
        /// <summary>
        /// نوع شغل به فارسی
        /// </summary>
        string CareerOpportunityFaDisplayName { get; set; }
         /// <summary>
        /// توضیحات شغل
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// شناسه تصویر
        /// </summary>

        Guid? CareerOpportunityImageGuid { get; set; }
        /// <summary>
        /// منبع عکس
        /// </summary>
        string CareerOpportunityImageSource { get; set; }

        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        bool IsActive { get; set; }

        ICollection<IResumeDto> ResumeDto { get; set; }
    }
}
