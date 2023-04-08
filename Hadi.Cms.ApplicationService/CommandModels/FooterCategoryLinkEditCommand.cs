using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش لینک فوتر
    /// </summary>
    public class FooterCategoryLinkEditCommand
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// شناسه دسته بندی فوتر
        /// </summary>
        public Guid FooterCategoryId { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Link { get; set; }
        /// <summary>
        /// متن لینک
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string LinkText { get; set; }
        /// <summary>
        /// شناسه عکس
        /// </summary>
        public Guid? ImageAttachmentGuid { get; set; }
        /// <summary>
        /// منبع عکس
        /// </summary>
        public string ImageAttachmentSource { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
