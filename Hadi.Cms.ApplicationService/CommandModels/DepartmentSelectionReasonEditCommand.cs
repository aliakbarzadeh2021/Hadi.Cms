using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class DepartmentSelectionReasonEditCommand
    {
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public Guid DepartmentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public string ImageSource { get; set; }
        public bool IsActive { get; set; }
    }
}
