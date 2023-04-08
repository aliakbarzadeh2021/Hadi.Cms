using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش رویداد
    /// </summary>
    public class EventEditCommand
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Link { get; set; }
        /// <summary>
        /// شناسه تصویر
        /// </summary>
        public Guid? AttachmentImageId { get; set; }
        /// <summary>
        /// منبع عکس
        /// </summary>
        public string AttachmentImageSource { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
