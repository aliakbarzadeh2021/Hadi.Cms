using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IFormDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// شناسه زبان فرم
        /// </summary>
        Guid LanguageId { get; set; }
        /// <summary>
        /// نام فرم
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// نام نمایشی فارسی
        /// </summary>
        string DisplayFaName { get; set; }
        /// <summary>
        /// نام نمایشی لاتین
        /// </summary>
        string DisplayEnName { get; set; }
        /// <summary>
        /// آدرس بازگشتی
        /// </summary>
        string RedirectUrl { get; set; }
        /// <summary>
        /// فیلد های فرم
        /// </summary>
        string FormDataSource { get; set; }

        string LanguageTitle { get; set; }

        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        ICollection<IFormFieldDto> FormFieldDto { get; set; }
    }
}
