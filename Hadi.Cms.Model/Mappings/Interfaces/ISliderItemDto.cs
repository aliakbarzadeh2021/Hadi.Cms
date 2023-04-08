using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ISliderItemDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// شناسه اسلایدر
        /// </summary>
        Guid SliderId { get; set; }
        /// <summary>
        /// ترتیب
        /// </summary>
        int OrderId { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// زیر عنوان
        /// </summary>
        string SubTitle { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// متن دکمه
        /// </summary>
        string ButtonText { get; set; }
        /// <summary>
        /// لینک دکمه
        /// </summary>
        string ButtonLink { get; set; }
        /// <summary>
        /// استایل های دکمه
        /// </summary>
        string ButtonCss { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        Guid? AttachmentImageId { get; set; }
        /// <summary>
        /// منبع عکس ضمیمه شده
        /// </summary>
        string AttachmentImageSource { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
         bool IsActive { get; set; }
         bool IsDeleted { get; set; }
        /// <summary>
        /// ایجاد شده توسط ...
        /// </summary>
        Guid CreatedBy { get; set; }
         ISliderDto SliderDto { get; set; }
    }
}
