using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت لینک
    /// </summary>
    public class FooterCategoryLinkCreateCommand
    {
        /// <summary>
        /// شناسه دسته بندی فوتر
        /// </summary>
        public Guid FooterCategoryId { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
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
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
