using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش ایتم اسلایدر
    /// </summary>
    public class SliderItemEditCommand
    {
        /// <summary>
        /// شناسه ایتم اسلایدر
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// شناسه اسلایدر
        /// </summary>
        public Guid SliderId { get; set; }
        /// <summary>
        /// ترتیب
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }
        /// <summary>
        /// زیر عنوان
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string SubTitle { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// متن دکمه
        /// </summary>
        public string ButtonText { get; set; }
        /// <summary>
        /// لینک دکمه
        /// </summary>
        public string ButtonLink { get; set; }
        /// <summary>
        /// استایل های دکمه
        /// </summary>
        public string ButtonCss { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? AttachmentImageId { get; set; }
        /// <summary>
        /// منبع عکس ضمیمه شده
        /// </summary>
        public string AttachmentImageSource { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
