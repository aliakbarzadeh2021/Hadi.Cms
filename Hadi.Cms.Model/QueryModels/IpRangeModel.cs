using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class IpRangeModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpRangeModel_Lower")]
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings) , ErrorMessageResourceName = "Required")]
        public string Lower { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpRangeModel_Upper")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Upper { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpRangeModel_IsActive")]
        public bool IsActive { get; set; }
    }
}
