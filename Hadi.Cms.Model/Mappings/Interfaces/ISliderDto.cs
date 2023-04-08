using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ISliderDto
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
        /// توضیحات
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// تاریخ و زمان ایحاد
        /// </summary>
        DateTime CreatedDate { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        bool IsActive { get; set; }
        /// <summary>
        /// ایجاد شده توسط ...
        /// </summary>
        Guid CreatedBy { get; set; }

        ICollection<ISliderItemDto> SliderItemDto { get; set; }
    }
}
