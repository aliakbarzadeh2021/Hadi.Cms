using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش کامنت
    /// </summary>
    public class ServiceCommentEditCommand
    {
        public Guid Id { get; set; }
        /// <summary>
        /// شناسه خدمت
        /// </summary>
        public Guid ServiceId { get; set; }
        /// <summary>
        /// متن نظر
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        /// <summary>
        /// نام کامل نظر دهنده
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string PersonFullName { get; set; }
        /// <summary>
        /// نقش نظر دهنده
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string PersonRoleTitle { get; set; }
        /// <summary>
        /// شناسه عکس
        /// </summary>
        public Guid? AttachmentImageId { get; set; }

        public string AttachmentImageSource { get; set; }
    }
}
