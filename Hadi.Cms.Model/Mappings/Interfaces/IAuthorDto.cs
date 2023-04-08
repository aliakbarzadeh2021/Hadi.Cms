using System;
using System.Collections;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IAuthorDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// نام کامل
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// شناسه تصویر نویسنده
        /// </summary>
        Guid? AuthorImageGuid { get; set; }
        /// <summary>
        /// منبع عکس نویسنده
        /// </summary>
        string AuthorImageSource { get; set; }
        /// <summary>
        /// آدرس اینستاگرام
        /// </summary>
        string InstagramAddress { get; set; }
        /// <summary>
        /// آدرس تلگرام
        /// </summary>
        string TelegramAddress { get; set; }
        /// <summary>
        /// آدرس لینکدین
        /// </summary>
        string LinkedInAddress { get; set; }
        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        ICollection<IArticleDto> ArticleDto { get; set; }
    }
}
