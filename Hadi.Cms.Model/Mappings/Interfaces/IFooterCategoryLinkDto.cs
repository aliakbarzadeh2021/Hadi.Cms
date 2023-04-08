using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IFooterCategoryLinkDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// شناسه دسته بندی فوتر
        /// </summary>
        Guid FooterCategoryId { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        string Link { get; set; }
        /// <summary>
        /// متن لینک
        /// </summary>
        string LinkText { get; set; }
        /// <summary>
        /// شناسه عکس
        /// </summary>
        Guid? ImageAttachmentGuid { get; set; }
        /// <summary>
        /// منبع عکس
        /// </summary>
        string ImageAttachmentSource { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        bool IsActive { get; set; }

        IFooterCategoryDto FooterCategoryDto { get; set; }
    }
}
