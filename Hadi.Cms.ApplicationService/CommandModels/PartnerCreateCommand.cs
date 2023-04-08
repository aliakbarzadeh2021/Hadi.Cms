using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت همکار جدید
    /// </summary>
    public class PartnerCreateCommand
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public Guid? AttachmentImageGuid { get; set; }
        public string Link { get; set; }
        public string ToolTip { get; set; }
        public bool IsActive { get; set; }
    }
}
