using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class NlMessageEditCommand
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Text { get; set; }
        public Guid? AttachmentId { get; set; }
        public string AttachmentSource { get; set; }
    }
}
