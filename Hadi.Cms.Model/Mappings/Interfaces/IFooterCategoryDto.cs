using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IFooterCategoryDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        string Link { get; set; }
        /// <summary>
        /// نمایش آماری
        /// </summary>
        bool StatisticalRepresentation { get; set; }
        /// <summary>
        /// تعداد نمایشی
        /// </summary>
        int NumberOfShows { get; set; }
        /// <summary>
        /// ترتیب
        /// </summary>
        int OrderId { get; set; }

        bool IsActive { get; set; }

        int? EntityHaveReviewCount { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        ICollection<IFooterCategoryLinkDto> FooterCategoryLinkDto { get; set; }

    }
}
