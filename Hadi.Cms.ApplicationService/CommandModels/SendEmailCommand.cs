using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// ارسال ایمیل
    /// </summary>
    public class SendEmailCommand
    {
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Subject { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public Guid? AttachmentId { get; set; }
    }
}
