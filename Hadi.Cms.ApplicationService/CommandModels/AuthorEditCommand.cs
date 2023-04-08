using Hadi.Cms.Language.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش اطلاعات نویسنده
    /// </summary>
    public class AuthorEditCommand
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }
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
        /// منبع عکس نویسنده
        /// </summary>
        public string AuthorImageSource { get; set; }
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
