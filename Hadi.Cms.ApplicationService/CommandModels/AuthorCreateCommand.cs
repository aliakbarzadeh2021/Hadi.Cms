using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت نویسنده
    /// </summary>
    public class AuthorCreateCommand
    {
        /// <summary>
        /// نام کامل
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }
        /// <summary>
        /// شناسه تصویر نویسنده
        /// </summary>
        public Guid? AuthorImageGuid { get; set; }
        /// <summary>
        /// آدرس اینستاگرام
        /// </summary>
        public string InstagramAddress { get; set; }
        /// <summary>
        /// آدرس تلگرام
        /// </summary>
        public string TelegramAddress { get; set; }
        /// <summary>
        /// آدرس لینکدین
        /// </summary>
        public string LinkedInAddress { get; set; }
    }
}
