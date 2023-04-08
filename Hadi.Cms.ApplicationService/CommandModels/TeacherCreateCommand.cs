using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت مدرس
    /// </summary>
    public class TeacherCreateCommand
    {
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public bool IsActive { get; set; }
        public string SocialNetworkName { get; set; }
        public string SocialNetworkLink { get; set; }
        public Guid? SocialNetworkImageGuid { get; set; }
    }
}
