using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// ایتم های اسلایدر
    /// </summary>
    public class SliderItem : BaseModel
    {
        public SliderItem()
        {
            
        }

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
        public string Title { get; set; }
        /// <summary>
        /// زیر عنوان
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
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
        public Slider Slider { get; set; }
    }
}
