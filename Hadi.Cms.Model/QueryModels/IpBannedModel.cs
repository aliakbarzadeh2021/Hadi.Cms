using Hadi.Cms.Language.Resources;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class IpBannedModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpBannedModel_IPAddress")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string IpAddress { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpBannedModel_IPAddressBanReason")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string IpAddressBanReason { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpBannedModel_IsActive")]
        public bool IsActive { get; set; }
    }
}
