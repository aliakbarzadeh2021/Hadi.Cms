using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش همکار
    /// </summary>
    public class PartnerEditCommand
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public Guid? AttachmentImageGuid { get; set; }
        public string AttachmentImageSource { get; set; }
        public string Link { get; set; }
        public string ToolTip { get; set; }
        public bool IsActive { get; set; }
    }
}
